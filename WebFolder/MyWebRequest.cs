using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace WebFolder
{
    class MyWebRequest
    {
		String url;
        Url myurl;
		public MyWebRequest(string s)
		{
			url = s;
		}
        public MyWebRequest(Url u)
        {
            myurl = u;
            url = u.getNome();
        }

		public String Gettype()
		{
                WebRequest requestheader = (HttpWebRequest)WebRequest.Create(url);
                requestheader.Method = "HEAD";
                WebResponse response = requestheader.GetResponse();
                String tipo = response.ContentType;
                int i = tipo.IndexOf('/');
                tipo = tipo.Remove(0, i+1);
                response.Close();
                return tipo;
		}

        private string getFicheiro()
        {
            int i = 0;
            string[] indexes = new string[4];
            indexes[0] = ("index.aspx");
            indexes[1] = ("index.asp");
            indexes[2]= ("index.shtml");
            indexes[3] = ("index.html");
            string path;
            while(i<4)
            {
                path = url + "/" + indexes[i];
                try
                {
                    WebRequest requestheader = (HttpWebRequest)WebRequest.Create(path);
                    WebResponse response = requestheader.GetResponse();
                    response.Close();
                    return indexes[i];
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (404) Not Found.")
                        i++;
                    else
                        return indexes[i];
                }
                
            }
            return "";
        }

		public int Getsize()
		{
			WebRequest requestheader = (HttpWebRequest)WebRequest.Create(url);            
			requestheader.Method = "HEAD";
			WebResponse response = requestheader.GetResponse();
			int size = (int)(response.ContentLength);
			response.Close();
			return size;
		}

		public string GetPage()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url) as HttpWebRequest;
			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)  
			{  
				StreamReader reader = new StreamReader(response.GetResponseStream());
				return reader.ReadToEnd();
			}
		}

		public DateTime GetLastModified()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "HEAD";
            //request.Timeout = 2;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            DateTime data = response.LastModified;
            response.Close();
            return data;

		}




  	}
}



