namespace tinyweb.viewengine.parrot
{
    public static class View
    {
        public static ParrotResult<T> Parrot<T>(T model, string templatePath)
        {
            return new ParrotResult<T>(model, templatePath);
        }

        public static ParrotResult Parrot(string templatePath)
        {
            return new ParrotResult(templatePath);
        }
    }
}
