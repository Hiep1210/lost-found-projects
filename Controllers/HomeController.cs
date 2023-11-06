using _2._LostFoundProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.IO;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace _2._LostFoundProj.Controllers
{
    public class HomeController : Controller
    {
        LostFoundProjContext con = new LostFoundProjContext();
        public IActionResult Home()
        {
            var post = from p in con.Posts
                       join po in con.Pendings on p.PostId equals po.Id
                       join accou in con.Accounts on po.Poster equals accou.Id
                       where (po.Lostorfound == true) && (po.Processed == true)
                       select new
                       {
                           post = p,
                           pend = po,
                           acc = accou,
                       };
            ViewData["img"] = con.Imags.ToList();
            ViewData["post"] = post;
            return View();
        }
        public IActionResult Lost()
        {
            var post = from p in con.Posts
                       join po in con.Pendings on p.PostId equals po.Id
                       join accou in con.Accounts on po.Poster equals accou.Id
                       where (po.Lostorfound == false) && (po.Processed == true)
                       select new
                       {
                           post = p,
                           pend = po,
                           acc = accou,
                       };
            ViewData["img"] = con.Imags.ToList();
            ViewData["post"] = post;
            return View();
        }
        public IActionResult DeletePost(int id)
        {

            var st = con.Posts.First(x => x.Id == id);
            return View(st);
        }
        [HttpPost]
        public IActionResult DeletePost(Post post)
        {
            int id = post.Id;
            var comment = con.Comments.Where(x => x.PostId == id).ToList();
            foreach (var com in comment)
            {
                con.Comments.Remove(com);
            }
            con.Posts.Remove(post);
            con.SaveChanges();
            TempData["message"] = "Remove Post successfully";
            return RedirectToAction("Home");
        }
        [HttpPost]
        public IActionResult Search(IFormCollection f)
        {
            string[] split = f["search"].ToString().Split(' ');
            bool a = "abcdef".Contains("abc");
            var posts = from p in con.Posts
                        join po in con.Pendings on p.PostId equals po.Id
                        join accou in con.Accounts on po.Poster equals accou.Id
                        where (po.Lostorfound == true) && (po.Processed == true)
                        select new
                        {
                            post = p,
                            pend = po,
                            acc = accou,
                        };
            foreach (var c in split)
            {
                posts = posts.Where(x => x.pend.Caption.ToUpper().Contains(c.ToUpper()));
            }
            ViewData["img"] = con.Imags.ToList();
            ViewData["post"] = posts;
            return View("Home");
        }
        public IActionResult Profile(int id)
        {
            if (HttpContext.Session.GetInt32("user") == null)
            {
                TempData["message"] = "You need to log in to perform this task";
                return RedirectToAction("LogIn");
            }
            ViewData["acc"] = con.Accounts.Find(id);
            return View();
        }
        [HttpPost]
        public IActionResult Profile(IFormCollection f)
        {
            int id = int.Parse(f["id"].ToString());
            Account acc = con.Accounts.Find(id);
            //acc.Id = int.Parse(f["id"].ToString());
            acc.Username = f["user"].ToString();
            if (!String.IsNullOrEmpty(f["password"].ToString()))
                acc.Pass = f["password"].ToString();
            acc.Phone = f["phone"].ToString();
            acc.Dob = DateTime.Parse(f["dob"].ToString());
            con.Accounts.Update(acc);
            con.SaveChanges();
            TempData["message"] = "Update Successfully";
            //Account acc = new Account();
            //acc.Id = int.Parse(f["id"].ToString());
            //acc.Username = f["user"].ToString();
            //acc.Pass = con.Accounts.Find(acc.Id).Pass;
            //if (!String.IsNullOrEmpty(f["password"].ToString()))
            //    acc.Pass = f["password"].ToString();
            //acc.Phone = f["phone"].ToString();
            //acc.Dob = DateTime.Parse(f["dob"].ToString());
            //con.Accounts.Update(acc);
            //con.SaveChanges();
            //TempData["message"] = "Update Successfully";
            return RedirectToAction("Profile", new { id = HttpContext.Session.GetInt32("user") });
        }
        [HttpGet]
        public IActionResult Chat(int id)
        {
            if (HttpContext.Session.GetInt32("user") == null)
            {
                TempData["message"] = "You need to log in to perform this task";
                return RedirectToAction("LogIn");
            }
            int? user = HttpContext.Session.GetInt32("user");
            Chat chatroom = new Chat();
            List<Chat> list = con.Chats.Include(x => x.User1Navigation).Include(x => x.User2Navigation).Where(x => (x.User1 == user) || (x.User2 == user)).ToList();
            if (id == 0)
            {
                if (list.Count == 0)
                {
                    return View();
                }
                else
                {
                    chatroom = list[0];
                }
            }
            else
            {
                chatroom = con.Chats.Include(x => x.User1Navigation).Include(x => x.User2Navigation).FirstOrDefault(x => (x.User1 == user && x.User2 == id) || (x.User1 == id && x.User2 == user));
                if (chatroom == null)
                {
                    chatroom = new Chat();
                    chatroom.User1 = user;
                    chatroom.User2 = id;
                    con.Chats.Add(chatroom);
                    con.SaveChanges();
                }
            }
            var mess = con.Cmessages.Include(x => x.Room).Include(x => x.Sender).Where(x => x.RoomId == chatroom.Id).OrderBy(x => x.Sdate).ToList();
            ViewData["chatlist"] = list;
            ViewData["chat"] = con.Chats.Include(x => x.User1Navigation).Include(x => x.User2Navigation).FirstOrDefault(x => (x.User1 == user && x.User2 == id) || (x.User1 == id && x.User2 == user));
            ViewData["mess"] = mess;
            return View();
        }
        [HttpPost]
        public IActionResult Chat(IFormCollection f)
        {
            var chatId = int.Parse(f["room"].ToString());
            var senderId = HttpContext.Session.GetInt32("user");
            var content = f["mess"].ToString();
            var chat = con.Chats.Find(chatId);
            var sendToId = chat.User1;
            if (chat.User1 == senderId)
            {
                sendToId = chat.User2;
            }
            var mess = new Cmessage();
            mess.Mess = content;
            mess.Sdate = DateTime.Now;
            mess.SenderId = senderId;
            mess.RoomId = chatId;
            con.Add(mess);
            con.SaveChanges();
            return RedirectToAction("Chat", new { id = sendToId });
        }
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(Account m)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            string admin_name = configuration.GetSection("AppSettings:DefaultAccount:User").Value;
            string admin_password = configuration.GetSection("AppSettings:DefaultAccount:Password").Value;
            var p = con.Accounts.FirstOrDefault(member => member.Username == m.Username);
            if (p == null)
            {
                TempData["message"] = "Found no username";
                return View();
            }
            if (p.Pass != m.Pass)
            {
                TempData["message"] = "Password is wrong";
                return View();
            }
            HttpContext.Session.SetInt32("user", p.Id);
            if (admin_name == p.Username && admin_password == m.Pass)
            {
                HttpContext.Session.SetString("admin", "true");
            }
            return RedirectToAction("Home");
        }
        [HttpPost]
        //    "DefaultAccount": {
        //  "User": "admin",
        //  "Password": "admin123"
        //}
        public IActionResult SignUp(Account acc)
        {
            if (con.Accounts.Select(x => acc.Username == x.Username) == null)
            {
                con.Accounts.Add(acc);
                con.SaveChanges();
                HttpContext.Session.SetInt32("user", acc.Id);
                return RedirectToAction("Home");
            }
            else
            {
                TempData["message"] = "Duplicate Username";
                return View("SignUp");
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("LogIn");
        }
        public IActionResult UserPost(int id)
        {

            return View();
        }
        [HttpGet]
        public IActionResult Comment(int id)
        {
            var post = con.Posts.Include(x => x.PostNavigation).FirstOrDefault(x => x.Id == id);
            ViewData["post"] = post;
            ViewData["acc"] = con.Accounts.FirstOrDefault(x => x.Id == post.PostNavigation.Poster);
            ViewData["comment"] = con.Comments.Include(x => x.User).Where(x => x.PostId == id).ToList();
            ViewData["img"] = con.Imags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Comment(IFormCollection f)
        {
            if (HttpContext.Session.GetInt32("user") == null)
            {
                TempData["message"] = "You need to log in to perform this task";
                return RedirectToAction("LogIn");
            }
            int id = int.Parse(f["post"]);
            Comment com = new Comment();
            com.UserId = HttpContext.Session.GetInt32("user");
            var acc = con.Accounts.Find(HttpContext.Session.GetInt32("user"));
            com.Com = f["com"].ToString();
            com.PostId = id;
            con.Comments.Add(com);
            con.SaveChanges();
            return RedirectToAction("Comment", new { id = id });
        }
        [HttpGet]
        public IActionResult CreateFound()
        {
            if (HttpContext.Session.GetInt32("user") == null)
            {
                TempData["message"] = "You need to log in to perform this task";
                return RedirectToAction("LogIn");
            }
            ViewData["type"] = con.ItemTypes.Select(x => x.Itype).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFoundAsync(IFormCollection f)
        {
            var imgs = f["imgs"];
            var caption = f["caption"].ToString();
            Pending pend = new Pending();
            pend.Poster = HttpContext.Session.GetInt32("user");
            pend.Postdate = DateTime.Now;
            pend.Caption = caption;
            pend.Lostorfound = true;
            pend.Processed = false;
            con.Pendings.Add(pend);
            con.SaveChanges();
            foreach (var file in f.Files)
            {
                if (file.Length > 0)
                {
                    Imag img = new Imag();
                    img.ImgName = file.FileName;
                    img.PostId = con.Pendings.ToList().Last().Id;
                    con.Imags.Add(img);
                    con.SaveChanges();
                    var infile = Path.Combine(@"wwwroot\img", file.FileName);

                    using (var stream = System.IO.File.Create(infile))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            TempData["message"] = "Post successfully";
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult ViewPost()
        {
            int? id = HttpContext.Session.GetInt32("user");
            var userPost = con.Posts.Include(x=>x.PostNavigation).Where(x => x.PostNavigation.PosterNavigation.Id == id);
            return View(userPost);
        }
        //admin function
        [HttpGet]
        public IActionResult Duyet()
        {
            var ob = con.Pendings.Where(x => x.Processed == false).Include(x => x.PosterNavigation).ToList();
            var img = con.Imags.ToList();
            ViewData["pend"] = ob;
            ViewData["img"] = img;
            return View();
        }
        [HttpPost]
        public IActionResult Duyet(IFormCollection f)
        {
            int pender = int.Parse(f["pender"].ToString());
            var pend = con.Pendings.Find(pender);
            if (!String.IsNullOrEmpty(f["app"].ToString()))
            {
                pend.Processed = true;
                Post post = new Post();
                post.PostId = pender;
                post.PostNavigation = pend;
                con.Posts.Add(post);
                con.SaveChanges();
            }
            else
            {
                pend.Processed = null;
            }
            TempData["message"] = "Process successfully";
            return RedirectToAction("Duyet");
        }
        public IActionResult MemberList()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                TempData["message"] = "You're not authorize as admin!";
                return View("LogIn");
            }
            var acc = con.Accounts.ToList();
            return View(acc);
        }
        public IActionResult BanUser(int id)
        {
            var st = con.Accounts.First(x => x.Id == id);
            return View(st);
        }
        [HttpPost]
        public IActionResult BanUser(Account acc)
        {
            con.Accounts.Remove(acc);
            con.SaveChanges();
            TempData["message"] = "Ban User Successfully";
            return RedirectToAction("MemberList");
        }

    }
}
