using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using theWall.Factory;
using theWall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace theWall.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserFactory userFactory;
        private readonly MsgFactory msgFactory;
        private readonly ComFactory comFactory;
        public UsersController()
        {
            userFactory = new UserFactory();
            msgFactory = new MsgFactory();
            comFactory = new ComFactory();
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.temperror = TempData["error"];
            ViewBag.error = "";
            return View();
        }


        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User user)
        {
                if(userFactory.FindByEmail(user.email)== null){
                    if(ModelState.IsValid)
                    {  
                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        user.pwd = Hasher.HashPassword(user, user.pwd);
                        userFactory.Add(user); 
                        var founduser = userFactory.FindByEmail(user.email);
                        HttpContext.Session.SetInt32("userId", founduser.id); 
                        HttpContext.Session.SetString("fname", user.fname);    
                        return RedirectToAction("Wall");
                    }
                    return View("Index");
                }  
                else{
                ViewBag.error = "User already exists";
                return View("Index");
                }
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string EmailTocheck, string PwdTocheck)
        {
                var Checkuser = userFactory.FindByEmail(EmailTocheck);
                if(Checkuser!=null && PwdTocheck !=null)
                {
                    var Hasher = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(Checkuser, Checkuser.pwd, PwdTocheck))
                    {                   
                        HttpContext.Session.SetInt32("userId", Checkuser.id);
                        HttpContext.Session.SetString("fname", Checkuser.fname);
                        return RedirectToAction("Wall");
                    }   
                    TempData["error"] = "Invalid email/password";
                    return RedirectToAction("Index");     //use tempdata with redirect and viebag with View()      
                } 
                TempData["error"] = "Invalid email/password";
                return RedirectToAction("Index");               
        }  
            

        [HttpGet]
        [Route("/wall")]
        public IActionResult Wall()
        {        
                if(!CheckLogin())
                {
                  return RedirectToAction("Index");  
                }
                ViewBag.item = new List<string>();
                ViewBag.messages = new List<Msg>();
                ViewBag.comments = new List<Com>();
                ViewBag.fname = HttpContext.Session.GetString("fname");
                ViewBag.messages = msgFactory.FindAll();              
                ViewBag.comments = comFactory.FindAll();    
                return View("Wall");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Postmsg")]
        public IActionResult Postmsg(Msg msg){
            if(!CheckLogin())
            {
                return RedirectToAction("Index");  
            }
            if(ModelState.IsValid)
            {   
                msg.userid = (int)HttpContext.Session.GetInt32("userId");
                msgFactory.Add(msg);
                return RedirectToAction("Wall");
            }
            else
            {
                ViewBag.item = ModelState.Values;   
                return View("Wall");
            }
        }

        [HttpPost]
        [Route("Postcom/{msgid}")]
        public IActionResult Postcom(Com com, int msgid){
            if(!CheckLogin())
            {
                return RedirectToAction("Index");  
            }
            if(ModelState.IsValid)
            {   
                com.userid = (int)HttpContext.Session.GetInt32("userId");
                com.msgid = msgid;
                comFactory.Add(com);
                
                     
                return RedirectToAction("Wall");
            }
            else
            {
                ViewBag.item = ModelState.Values;   
                return View("Wall");
            }
        }
        private bool CheckLogin()
        {
            return (HttpContext.Session.GetInt32("userId") != null);
        }

        
    }
}
