namespace Parrot.TinyWeb
{
    using Parrot.Infrastructure;
    using Parrot.Renderers;
    using Parrot.Renderers.Infrastructure;

    public class TinyWebHost : IHost
    {
        public TinyWebHost()
        {
            ModelValueProviderFactory = new ModelValueProviderFactory(new ValueTypeProvider());
            RendererFactory = new RendererFactory(new IRenderer[]
                {
                    new HtmlRenderer(this),
                    new StringLiteralRenderer(this),
                    new DocTypeRenderer(this),
                    new ForeachRenderer(this),
                    new InputRenderer(this),
                    new ConditionalRenderer(this),
                    new ListRenderer(this),
                    new SelfClosingRenderer(this),
                });
                PathResolver = new PathResolver()
            ;
        }

        public IParrotWriter CreateWriter()
        {
            return new PrettyStringWriter();
        }

        public IModelValueProviderFactory ModelValueProviderFactory { get; set; }
        public IRendererFactory RendererFactory { get; set; }
        public IPathResolver PathResolver { get; set; }
    }
}
