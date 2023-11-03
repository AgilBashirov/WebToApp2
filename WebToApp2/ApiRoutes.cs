namespace WebToApp2
{
    public struct ApiRoutes
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;

        public struct Auth
        {
            public const string GenerateQr = Base + "/generateQr";
            public const string GetFile = Base + "/getFile";
            public const string Callback = Base + "/callback";
            public const string DataUri = Base + "/getData";
        }
    }
}
