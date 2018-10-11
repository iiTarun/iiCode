using System.Web.Mvc;

namespace Nom1Done.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        //public static string StringToParagraphs(string input, int rowLength = 250)
        //{

        //    //bool isParagraphFinished = false;

        //    StringBuilder result = new StringBuilder();

        //    StringBuilder line = new StringBuilder();



        //    Queue<string> stack = new Queue<string>(input.Split(' '));



        //    while (stack.Count > 0)

        //    {

        //        var word = stack.Dequeue();

        //        if (word.Length > rowLength && word.EndsWith("."))

        //        {

        //            string head = word.Substring(0, rowLength);

        //            string tail = word.Substring(rowLength);



        //            word = head;

        //            stack.Enqueue(tail);

        //        }



        //        if ((line.Length + word.Length) >= rowLength && word.EndsWith("."))

        //        {

        //            //stack.Pop();

        //            line.Append(word + " ");

        //            result.AppendLine(line.ToString() + "\n\n");

        //            line.Clear();

        //            //      isParagraphFinished = true;

        //        }

        //        else

        //        {

        //            line.Append(word + " ");

        //        }



        //        //if(line.Length > rowLength * 10)

        //        //{

        //        //    result.AppendLine(line.ToString());

        //        //    line.Clear();

        //        //}

        //        //if(isParagraphFinished)



        //    }



        //    result.Append(line);

        //    return result.ToString();

        //}


    }
}