using Ipfs.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Getting_Startted_with_IPFS
{
    internal class Program
    {
        static readonly IpfsClient ipfs = new IpfsClient("http://127.0.0.1:5001");
        static async Task Main(string[] args)
        {
            int choice;
            string cont;
            Console.WriteLine("                  **************        Choose       ****************                ");
            Console.WriteLine("1 to Upload Image to IPFS from file Location ");
            Console.WriteLine("2 to Upload Image to IPFS from External link ");

            choice = Convert.ToInt32(Console.ReadLine());
            do
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\nProvide the file location of the Image : ");
                        string fileLocation = Console.ReadLine();

                        UploadFromPath(fileLocation).Wait();
                        break;

                    case 2:
                        Console.WriteLine("\nProvide the file URL of the Image : ");
                        string fileUrl = Console.ReadLine();

                        UploadFromUrl(fileUrl).Wait();
                        break;

                    default:
                        Console.WriteLine("Wrong Choice");
                        break;
                }

                Console.WriteLine("\n\nDo you want to continue : Y / N");
                cont = Console.ReadLine();
            }
            while (cont.Equals("Y") ||cont.Equals("y") || cont.Equals("Yes") || cont.Equals("yes"));

            Console.WriteLine("Thank YOU ");
            Console.ReadLine();
        }

        /// <summary>
        /// This function takes the file path as argument and upload the file to IPFS
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public async Task<bool> UploadFromPath(string path)
        {
            try
            {
                byte[] imageInBytes = File.ReadAllBytes(path);

                MemoryStream stream = new MemoryStream(imageInBytes);

                var upload = await ipfs.FileSystem.AddAsync(stream);        // uploading image to ipfs
                string cid = upload.Id.ToString();                          // extracting the CID

                string ipfsUrl = $"https://ipfs.io/ipfs/{cid}";             // ipfs url 

                Console.WriteLine($"Your IPFS URL : {ipfsUrl}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// This function takes file URL as argument and upload file to IPFS
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public async Task<bool> UploadFromUrl(string url)
        {
            try
            {
                byte[] imageData;
                var webClient = new WebClient();
                imageData = webClient.DownloadData(url);

                MemoryStream stream = new MemoryStream(imageData);

                var upload = await ipfs.FileSystem.AddAsync(stream);
                string cid = upload.Id.ToString();
                string ipfsUrl = $"https://ipfs.io/ipfs/{cid}";             // ipfs url 

                Console.WriteLine($"Your IPFS URL : {ipfsUrl}");

                return true;
            }
            catch(Exception ex)  
            { 
                Console.WriteLine(ex.Message.ToString());   
                return false; 
            } 
        }
    }
}
