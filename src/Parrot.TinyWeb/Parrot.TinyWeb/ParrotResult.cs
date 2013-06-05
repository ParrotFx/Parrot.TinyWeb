namespace tinyweb.viewengine.parrot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Parrot.Infrastructure;
    using Parrot.Nodes;
    using Parrot.Parser;
    using Parrot.Renderers;
    using Parrot.TinyWeb;
    using tinyweb.framework;

    public class ParrotResult<T> : ParrotResult
    {
        public ParrotResult(T model, string templateName) : base(templateName, model) { }
    }

    public class ParrotResult : IResult
    {
        private readonly string _templateName;
        private readonly object _model;
        private readonly TinyWebHost _host;

        public ParrotResult(string templateName, object model = null)
        {
            _templateName = templateName;
            _model = model;
            _host = new TinyWebHost();
        }

        public virtual void ProcessResult(IRequestContext request, IResponseContext response)
        {
            var contents = Parse(request, LoadContent());

            response.ContentType = "text/html";
            response.Write(contents);
        }

        internal Document LoadDocument(string template)
        {
            Parser parser = new Parser();
            Document document;

            if (parser.Parse(template, out document))
            {
                return document;
            }

            throw new Exception("Unable to parse: ");
        }

        private string Parse(IRequestContext request, string loadContent)
        {
            Document document = LoadDocument(loadContent);
            if (document.Errors.Any())
            {
                throw new Exception(string.Join("\r\n", document.Errors));
            }
            
            var documentHost = new Dictionary<string, object>();
            documentHost.Add("Model", _model);
            documentHost.Add("Request", HttpContext.Current.Request);
            documentHost.Add("Session", HttpContext.Current.Session);
            documentHost.Add("Response", HttpContext.Current.Response);
            documentHost.Add("Cache", HttpContext.Current.Cache);
            documentHost.Add("User", HttpContext.Current.User);

            //need to create a custom viewhost
            var rendererFactory = _host.RendererFactory;
            DocumentView view = new DocumentView(_host, rendererFactory, documentHost, document);

            IParrotWriter writer = _host.CreateWriter();
            view.Render(writer);
            
            return writer.Result();
        }

        public string LoadContent()
        {
            using (var stream = _host.PathResolver.OpenFile(_templateName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}