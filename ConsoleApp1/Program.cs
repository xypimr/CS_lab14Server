using System.Net.Http.Json;
using WebApplication1;

class Program
{
    static void Main()
    {
        HttpClient client = new HttpClient( );
        client.BaseAddress = new Uri("http://localhost:5204");
        void viewdetail()
        {
            string url = "http://localhost:5204/api/Detail/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }

        void alldetails()
        {
            
            string url = "http://localhost:5204/api/Details";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1); 
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }

        void addDetail()
        {
            Detail temp = new Detail() {Name = "test2", Quantity = 123};
            
            Task<HttpResponseMessage> request =  client.PostAsJsonAsync(
                $"api/Detail", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }

        void changedetail()
        {
            Detail temp = new Detail() {Name = "test3", Quantity = 321, Id = 3};
            
            Task<HttpResponseMessage> request =  client.PutAsJsonAsync(
                $"api/Detail", temp);
            string ans = request.Result.StatusCode.ToString();
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            Console.Write(ans);
        }

        void deletedetail()
        {
            
            string url = "http://localhost:5204/api/Detail/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
        
        void viewAssembly()
        {
            
            string url = "http://localhost:5204/api/Assembly/1";
            Task<HttpResponseMessage> request = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
           
        void allassemblies()
        {
            
            string url = "http://localhost:5204/api/Assemblies";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.WriteLine(data1); 
            string ans = request.Result.StatusCode.ToString();
            Console.Write(ans);
        }
        void addassembly()
        {
            
            PartView a = new PartView() {DetailName = "test1", DetailId = 1, Quantity = 5};
            PartView b = new PartView() {DetailName = "test2", DetailId = 2, Quantity = 5};
            AssemblyView temp = new AssemblyView() {name = "test1", PartViews = {a, b}};
            Task<HttpResponseMessage> request =  client.PostAsJsonAsync(
                $"api/Assembly", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }
        
        void changessembly()
        {
            
            PartView a = new PartView() {DetailName = "test1", DetailId = 1, Quantity = 12};
            PartView b = new PartView() {DetailName = "test2", DetailId = 2, Quantity = 12};
            AssemblyView temp = new AssemblyView() {name = "test1", PartViews = {a, b}, id = 1};
            Task<HttpResponseMessage> request =  client.PutAsJsonAsync(
                $"api/Assembly", temp);
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(data1);
            Console.Write(ans);
        }
        
        void deleteassembly()
        {
            
            string url = "http://localhost:5204/api/Assembly/2";
            Task<HttpResponseMessage> request = new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            Task<Stream> stream1 = request.Result.Content.ReadAsStreamAsync();
            StreamReader sr1 = new StreamReader(stream1.Result);
            string data1 = sr1.ReadToEnd();
            Console.Write(data1);
            string ans = request.Result.StatusCode.ToString();
            Console.WriteLine(ans);
        }
        //alldetails();
        //viewdetail();
        //addDetail();
        //deletedetail();
        //viewdetail();
        //alldetails();
        //alldetails();
        //deleteassembly();
        //addassembly();
        //viewAssembly();
        // allassemblies();
        //changessembly();
    }
}