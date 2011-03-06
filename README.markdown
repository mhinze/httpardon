####httpardon - httparty for .net

    var response = Http.get("http://twitter.com/statuses/public_timeline.json")

    Console.WriteLine("{1} {0}", response.Raw, response.HttpWebRequest.StatusCode)
    foreach (var x in response.Body) Console.WriteLine(x.user.screen_name); 


####wut - dsl for async httplistener, great for testing httpardon

    Listen.OnLocalhost().Respond(x => x.Json(twitter_json));
    var response = Http.get(Listen.Url)
    Listen.Stop();
