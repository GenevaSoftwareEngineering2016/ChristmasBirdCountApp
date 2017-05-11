using System.Net;
using System.IO;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    string myStuff = "stuff";
    List<String> birdList = new List<String>();

    //var provider = new MultipartMemoryStreamProvider();
    //string filename = "";

    // Get request body
    //dynamic data = req.Content.ReadAsMultipartAsync(provider).

    try{

        if (req.Content.IsMimeMultipartContent())
        {
            myStuff += " - Inside"; 

            var streamProvider = new MultipartMemoryStreamProvider();
            var task = req.Content.ReadAsMultipartAsync(streamProvider).
                ContinueWith(t =>
                    {
                        var fileContent = streamProvider.Contents.SingleOrDefault();

                        if(fileContent != null)
                        {
                            var fileName = fileContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                        }
                });

            log.Info(task.ToString());
        }

    } catch(Exception e) {
        log.Info(e.ToString());
    }
    //log.Info(data);

    
    log.Info(myStuff);


    //parse csv file
   // List<String> birdList = new List<String>();
    //try{
    //        using (StreamReader readFile = new StreamReader(File.OpenRead(data)))
    //        {
    //            string line;
    //            string[] row;
    //            int i = 0;

    //            while ((line = readFile.ReadLine()) != null)
    //            {
    //                row = line.Split(',');
     //               birdList.Add(row[i]);
    //                i++;
     //           }
    //        }

            //myStuff =  data;//birdList[0].ToString();

    //} catch(Exception e) {
    //    myStuff = e.ToString();
    //}

    // Set name to query string or body data
    //name = name ?? data?.name;
    

    return myStuff == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Bad Stuff")
        : req.CreateResponse(HttpStatusCode.OK, myStuff);
}