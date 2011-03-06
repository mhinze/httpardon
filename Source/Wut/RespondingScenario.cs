namespace Wut
{
    internal class RespondingScenario : IRespondingScenario
    {
        public string Body { get; set; }
        public string ContentType { get; set; }

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