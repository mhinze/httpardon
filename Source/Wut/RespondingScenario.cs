namespace Wut
{
    internal class RespondingScenario : IRespondingScenario
    {
        public string Body { get; private set; }
        public string ContentType { get; private set; }

        void IRespondingScenario.Default()
        {
            Body = "response body";
            ContentType = "text/plain";
        }

        void IRespondingScenario.Body(string body)
        {
            Body = body;
        }

        void IRespondingScenario.ContentType(string contentType)
        {
            ContentType = contentType;
        }

        void IRespondingScenario.Json(string json)
        {
            Body = json;
            ContentType = "application/json";
        }
    }
}