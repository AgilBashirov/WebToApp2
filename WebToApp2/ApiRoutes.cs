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

        public struct File
        {
            public const string GetAll = Base + "/files";
            public const string Get = Base + "/files/{id}";
            public const string Create = Base + "/files";
            public const string Update = Base + "/files/{id}";
            public const string Delete = Base + "/files/{id}";
        }
    }
}
