using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiktok
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rd = new Random();
        DataTable table = new DataTable();
        DataTable tb1 = new DataTable();
        DataTable tb2 = new DataTable();
        DataTable tb3 = new DataTable();
        string ExtentionFolderPath = "Extention";
        string fnameProxy = "..//..//Data//Proxy.txt";
        string fnameUa = "..\\..\\Data\\Ua.txt";
        string fnameBusiness = "..//..//Data//Businessname.txt";
        string fnameWebsite = "..//..//Data//Website.txt";
        string fnameHotmail = "..//..//Data//Hotmail.txt";
        string fnamelog = "..//..//Data//Log//";
        string fnameKey = "..//..//Data//key.txt";
        string fnameKeyApi = "..//..//Data//keyapi.txt";
        string fnameAccount = "..//..//Data//BackupAcc.txt";
        string fnamePhone = "..//..//Data//Phone.txt";
        string fnameTw = "..//..//Data//tktw.txt";
        string fnameBackupAccTW = "..//..//Data//BackupAccTW.txt";
        string fnameAccPs = "..//..//Data//AccPs.txt";
        string fnameHotmailPs = "..//..//Data//HotmailPs.txt";
        string fnameTukhoa = "..//..//Data//Tukhoa.txt";
        string fnameLinkgioithieu = "..//..//Data//Linkgioithieu.txt";
        private string Removelissfilehotmai(int index)
        {
            List<string> quotelist = File.ReadAllLines(fnameUa).ToList();
            string firstItem = quotelist[index];
            quotelist.RemoveAt(index);
            File.WriteAllLines(fnameUa, quotelist.ToArray());

            //đọc lại ua vào richtextbox
            if (fnameUa.Length != 0)
            {
                rtbUA.Text = File.ReadAllText(fnameUa);
            }
            return firstItem;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;
            string[] checkUa = File.ReadAllLines(fnameUa);
            int checkua = checkUa.Length;
            string[] checkHotmail = File.ReadAllLines(fnameHotmail);
            int checkhotmail = checkHotmail.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Email = null;
            string Passmail = null;
            int luong = (int)numThread.Value;
            int dem = 0;

            if (rdbtnKhongdungproxy.Checked)
            {
                int demip = -1;
                if (btnStart.Text == "Start")
                {
                    btnStart.Text = "Stop";
                    btnStart.BackColor = Color.FromArgb(255, 43, 42);
                    for (int i = 0; i < luong; i++)
                    {
                        var index = i;
                        int y = 900 * index;
                        for (int j = 0; j < 3; j++)
                        {
                            demip++;
                            var indexj = j;
                            int x = 650 * indexj;
                            dem++;
                            if (dem > luong)
                                break;

                            if (demip == dataGridViewTaikhoanreg.Rows.Count)
                                break;

                            //Get UA
                            if (checkBoxKhongua.Checked)
                            {
                                Useragent = null;
                            }
                            else
                            {
                                if (cbxoaUasaukhidung.Checked)
                                {
                                    checkUa = File.ReadAllLines(fnameUa);
                                    checkua = checkUa.Length;
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = Removelissfilehotmai(indexua);
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                                else
                                {
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = checkUa[indexua];
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                            }

                            Email = dataGridViewTaikhoanreg.Rows[demip].Cells[0].Value.ToString();
                            Passmail = dataGridViewTaikhoanreg.Rows[demip].Cells[1].Value.ToString();
                            Thread t = new Thread(() =>
                            {
                                Seleniumm(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Passmail);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.3));
                        }
                    }
                }
                else
                {
                    btnStart.Text = "Start";
                    btnStart.BackColor = Color.FromArgb(75, 201, 67);
                    try
                    {
                        Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                        foreach (Process processChrome in processesChrome)
                        {

                            processChrome.Kill();
                        }
                    }
                    catch { }
                }
            }
            else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
            {
                if (rdbtnDungriengproxy.Checked)
                {
                    int demip = -1;
                    if (btnStart.Text == "Start")
                    {
                        btnStart.Text = "Stop";
                        btnStart.BackColor = Color.FromArgb(255, 43, 42);
                        for (int i = 0; i < luong; i++)
                        {
                            var index = i;
                            int y = 900 * index;
                            for (int j = 0; j < 3; j++)
                            {
                                demip++;
                                var indexj = j;
                                int x = 650 * indexj;
                                dem++;
                                if (dem > luong)
                                    break;

                                string[] Allproxy = checkProxy[demip].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Email = dataGridViewTaikhoanreg.Rows[demip].Cells[0].Value.ToString();
                                Passmail = dataGridViewTaikhoanreg.Rows[demip].Cells[1].Value.ToString();

                                Thread t = new Thread(() =>
                                {
                                    Seleniumm(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Passmail);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }

                    }
                    else
                    {
                        btnStart.Text = "Start";
                        btnStart.BackColor = Color.FromArgb(75, 201, 67);
                        try
                        {
                            Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                            foreach (Process processChrome in processesChrome)
                            {

                                processChrome.Kill();
                            }
                        }
                        catch { }
                    }
                }
                else if (rdbtnDungchungproxy.Checked)
                {
                    int demip = -1;
                    if (btnStart.Text == "Start")
                    {
                        btnStart.Text = "Stop";
                        btnStart.BackColor = Color.FromArgb(255, 43, 42);
                        for (int i = 0; i < luong; i++)
                        {
                            var index = i;
                            int y = 900 * index;
                            for (int j = 0; j < 3; j++)
                            {
                                demip++;
                                var indexj = j;
                                int x = 650 * indexj;
                                dem++;
                                if (dem > luong)
                                    break;
                                if (demip == dataGridViewTaikhoanreg.Rows.Count)
                                    break;
                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Email = dataGridViewTaikhoanreg.Rows[demip].Cells[0].Value.ToString();
                                Passmail = dataGridViewTaikhoanreg.Rows[demip].Cells[1].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    Seleniumm(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Passmail);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }
                    }
                    else
                    {
                        btnStart.Text = "Start";
                        btnStart.BackColor = Color.FromArgb(75, 201, 67);
                        try
                        {
                            Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                            foreach (Process processChrome in processesChrome)
                            {

                                processChrome.Kill();
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void SeleniumPs(int x1, int y1, string ipproxy, string userproxy, string passproxy, string useragent, string email, string nameProfile)
        {
            for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
            {
                if (dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString() == email)
                {
                    dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Đang tạo...";
                    dtgvTaiKhoanPs.Rows[i].Cells[5].Value = nameProfile;
                    dtgvTaiKhoanPs.Rows[i].Cells[6].Value = useragent;
                }
            }
            ChromeOptions chromeOptions = new ChromeOptions();
            bool checkip = false;

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            Invoke(new Action(() =>
            {
                //Add Extention cho chrome
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=800,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            //Tắt cửa sổ phụ hiện lên khi add extenion
            if (checkip == true)
            {
                ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                string firstTab = windowHandles.First();
                string lastTab = windowHandles.Last();
                chome.SwitchTo().Window(firstTab);
                chome.Close();
                chome.SwitchTo().Window(lastTab);
            }

            //Thực hiện gán ip cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy))
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                        chome.Navigate();

                        chome.FindElementById("login").SendKeys(userproxy);
                        chome.FindElementById("password").SendKeys(passproxy);
                        chome.FindElementById("retry").Clear();
                        chome.FindElementById("retry").SendKeys("2");
                        chome.FindElementById("save").Click();
                    }
                }
            }));
            try
            {
                //Vào việc
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Đang đăng ký...";
                    }
                }
                chome.Navigate().GoToUrl(txtLinkgioithieu.Text);

                //Ấn nút Joid
                int d0 = chome.FindElementsByCssSelector(".btn.btn-lg.btn-primary").Count();
                while (d0 == 0)
                {
                    if (chome.FindElementsByCssSelector(".btn.btn-lg.btn-primary").Count() > 0)
                    {
                        d0++;
                    }
                }
                chome.FindElementByCssSelector(".btn.btn-lg.btn-primary").Click();
                Thread.Sleep(1500);

                //Bắt đầu đăng ký
                int d1 = chome.FindElementsByXPath("//button[contains(text(),'Sign Up Now')]").Count();
                while (d1 == 0)
                {
                    if (chome.FindElementsByXPath("//button[contains(text(),'Sign Up Now')]").Count() > 0)
                    {
                        d1++;
                    }
                }
                chome.FindElementByXPath("/html/body/div[1]/div[2]/div/div[2]/div[3]/div[2]/form/div[1]/input").SendKeys(email);
                Thread.Sleep(500);
                chome.FindElementByXPath("/html/body/div[1]/div[2]/div/div[2]/div[3]/div[2]/form/div[2]/input").SendKeys(txtPassPs.Text);
                Thread.Sleep(500);
                chome.FindElementById("password-confirm").SendKeys(txtPassPs.Text);
                Thread.Sleep(500);
                chome.FindElementByXPath("/html/body/div[1]/div[2]/div/div[2]/div[3]/div[2]/form/div[4]/label/input").Click();
                Thread.Sleep(500);
                var iframe = chome.FindElementByXPath("//body/div[@id='wrapper']/div[2]/div[1]/div[2]/div[3]/div[2]/form[1]/div[5]/div[1]/div[1]/div[1]/iframe[1]");
                chome.SwitchTo().Frame(iframe);
                chome.FindElementByXPath("/html/body/div[2]/div[3]/div[1]/div/div/span/div[1]").Click();// click vào capcha
                Thread.Sleep(2000);

                //Cập nhật status
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[2].Value = txtPassPs.Text;
                    }
                }

                //Chuyển sang cửa sổ chính
                ReadOnlyCollection<string> windowHandles1 = chome.WindowHandles;
                string firstTab1 = windowHandles1.First();
                string lastTab1 = windowHandles1.Last();
                chome.SwitchTo().Window(firstTab1);

                //Ấn skip
                int d2 = chome.FindElementsByXPath("/html/body/div[5]/div/div[5]/a[1]").Count();
                while (d2 == 0)
                {
                    if (chome.FindElementsByXPath("/html/body/div[5]/div/div[5]/a[1]").Count() > 0)
                    {
                        d2++;
                    }
                }
                chome.FindElementByXPath("/html/body/div[5]/div/div[5]/a[1]").Click();
                Thread.Sleep(3000);
                //Close aletter
                var confirm_win = chome.SwitchTo().Alert();
                confirm_win.Accept();
                Thread.Sleep(1000);

                //Cập nhật status
                //Email|Passmail|PassPre|Coin|Status|Profile|Ua|Id|Luotsr|check
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[3].Value = "25.00";
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Live";
                    }
                }

                //Thoát chome
                chome.Close();
                chome.Quit();
            }
            catch (Exception)
            {
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Not Live";
                    }
                }
                chome.Quit();
            }
        }

        private void SeleniummTw(int x1, int y1, string phone, string ipproxy, string userproxy, string passproxy, string useragent, string nameProfile)
        {
            for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
            {
                if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                {
                    dtgrvRegTw.Rows[i].Cells[4].Value = "Đang reg...";
                    dtgrvRegTw.Rows[i].Cells[3].Value = nameProfile;
                    dtgrvRegTw.Rows[i].Cells[6].Value = useragent;
                }
            }

            ChromeOptions chromeOptions = new ChromeOptions();
            bool checkip = false;
            string user = "";

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            Invoke(new Action(() =>
            {


                //Add Extention cho chrome
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=800,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            //Tắt cửa sổ phụ hiện lên khi add extenion
            if (checkip == true)
            {
                ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                string firstTab = windowHandles.First();
                string lastTab = windowHandles.Last();
                chome.SwitchTo().Window(firstTab);
                chome.Close();
                chome.SwitchTo().Window(lastTab);
            }

            //Thực hiện gán ip cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy))
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                        chome.Navigate();

                        chome.FindElementById("login").SendKeys(userproxy);
                        chome.FindElementById("password").SendKeys(passproxy);
                        chome.FindElementById("retry").Clear();
                        chome.FindElementById("retry").SendKeys("2");
                        chome.FindElementById("save").Click();
                    }
                }
            }));

            try
            {
                //Vào việc

                chome.Navigate().GoToUrl("https://twitter.com/i/flow/signup");
                Thread.Sleep(1000);
                //Điền Name
                int d0 = chome.FindElementsByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/label[1]/div[1]/div[2]/div[1]/input[1]").Count();
                while (d0 == 0)
                {
                    if (chome.FindElementsByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/label[1]/div[1]/div[2]/div[1]/input[1]").Count() > 0)
                    {
                        d0++;
                    }
                }
                string[] ArrBusiness = File.ReadAllLines(fnameBusiness);
                if (ArrBusiness.Length > 0)
                {
                    int abc = rd.Next(0, (ArrBusiness.Length) - 1);
                    chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/label[1]/div[1]/div[2]/div[1]/input[1]").SendKeys(ArrBusiness[abc]);
                }
                else
                {
                    chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/label[1]/div[1]/div[2]/div[1]/input[1]").SendKeys("Tú Thanh");
                }
                Thread.Sleep(500);
                //Điền phone
                chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[3]/label[1]/div[1]/div[2]/div[1]/input[1]").SendKeys(phone);

                //Điền ngày sinh
                int indexday = rd.Next(1, 31);
                int indexermonth = rd.Next(1, 12);
                int indexyear = rd.Next(1970, 2000);

                chome.FindElementById("SELECTOR_1").Click();
                Thread.Sleep(500);
                if (indexermonth == 1)
                {
                    chome.FindElementByXPath("//option[contains(text(),'January')]").Click();
                }
                if (indexermonth == 2)
                {
                    chome.FindElementByXPath("//option[contains(text(),'February')]").Click();
                }
                if (indexermonth == 3)
                {
                    chome.FindElementByXPath("//option[contains(text(),'March')]").Click();
                }
                if (indexermonth == 4)
                {
                    chome.FindElementByXPath("//option[contains(text(),'April')]").Click();
                }
                if (indexermonth == 5)
                {
                    chome.FindElementByXPath("//option[contains(text(),'May')]").Click();
                }
                if (indexermonth == 6)
                {
                    chome.FindElementByXPath("//option[contains(text(),'June')]").Click();
                }
                if (indexermonth == 7)
                {
                    chome.FindElementByXPath("//option[contains(text(),'July')]").Click();
                }
                if (indexermonth == 8)
                {
                    chome.FindElementByXPath("//option[contains(text(),'August')]").Click();
                }
                if (indexermonth == 9)
                {
                    chome.FindElementByXPath("//option[contains(text(),'September')]").Click();
                }
                if (indexermonth == 10)
                {
                    chome.FindElementByXPath("//option[contains(text(),'October')]").Click();
                }
                if (indexermonth == 11)
                {
                    chome.FindElementByXPath("//option[contains(text(),'November')]").Click();
                }
                if (indexermonth == 12)
                {
                    chome.FindElementByXPath("//option[contains(text(),'December')]").Click();
                }

                Thread.Sleep(500);
                chome.FindElementById("SELECTOR_2").Click();
                Thread.Sleep(500);
                chome.FindElementByXPath("//option[contains(text(),'" + indexday + "')]").Click();
                Thread.Sleep(500);
                chome.FindElementById("SELECTOR_3").Click();
                Thread.Sleep(500);
                chome.FindElementByXPath("//option[contains(text(),'" + indexyear + "')]").Click();
                Thread.Sleep(500);
                //Click next
                chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/div[1]").Click();
                Thread.Sleep(1000);
                //Click next
                chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]/div[1]").Click();
                Thread.Sleep(1000);
                //Click sigup
                chome.FindElementByXPath("//body/div[@id='react-root']/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[5]").Click();
                Thread.Sleep(1000);
                //Click OK
                chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[3]/div/div/div/div/div/div[2]/div[2]/div[2]/div[2]/div").Click();
                for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                {
                    if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                    {
                        dtgrvRegTw.Rows[i].Cells[4].Value = "Đang đợi code...";
                    }
                }
                Thread.Sleep(10000);
                //Lấy code
                string id = "";
                for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                {
                    if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                    {
                        id = dtgrvRegTw.Rows[i].Cells[0].Value.ToString();
                    }
                }
                ((IJavaScriptExecutor)chome).ExecuteScript("window.open();");
                chome.SwitchTo().Window(chome.WindowHandles.Last());
                bool checkcode = false;
                int dcheck = 0;
                string kq = "";
                while (checkcode == false)
                {
                    chome.Navigate().GoToUrl("https://chothuesimcode.com/api?act=code&apik=" + txtAPIPhone.Text + "&id=" + id);
                    Thread.Sleep(2000);
                    string rs = chome.PageSource;

                    rs = rs.Replace("\"", "*");
                    //Tách code

                    int indexcheck = rs.LastIndexOf("ResponseCode*:");
                    string check = rs.Substring(indexcheck + 14, 1);
                    if (check == "0")
                    {
                        int indxecode = rs.LastIndexOf("Code*:*");
                        kq = rs.Substring(indxecode + 7);
                        int incode = kq.LastIndexOf("*}}<");
                        kq = kq.Substring(0, incode);

                        checkcode = true;
                        break;
                    }
                    else
                    {
                        checkcode = false;
                        Thread.Sleep(10000);
                        dcheck++;
                        if (dcheck == 6)
                        {
                            checkcode = true;
                        }
                    }
                }
                if (dcheck == 6)
                {
                    for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                    {
                        if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                        {
                            dtgrvRegTw.Rows[i].Cells[4].Value = "Code không về...";
                        }
                    }
                    chome.Close();
                    chome.Quit();
                }
                else
                {
                    //Điền code
                    ReadOnlyCollection<string> windowHandles1 = chome.WindowHandles;
                    string firstTab1 = windowHandles1.First();
                    string lastTab1 = windowHandles1.Last();
                    chome.SwitchTo().Window(firstTab1);
                    Thread.Sleep(500);
                    chome.FindElementByCssSelector(".r-30o5oe.r-1niwhzg.r-17gur6a.r-1yadl64.r-deolkf.r-homxoj.r-poiln3.r-7cikom.r-1ny4l3l.r-t60dpp.r-1dz5y72.r-fdjqy7.r-13qz1uu").SendKeys(kq);
                    Thread.Sleep(500);
                    //Click next
                    chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Click();
                    for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                    {
                        if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                        {
                            dtgrvRegTw.Rows[i].Cells[4].Value = "Đã xm code...";
                        }
                    }
                    Thread.Sleep(1000);
                    //Điền mật khẩu
                    int d2 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[2]/div/label/div/div[2]/div/input").Count();
                    while (d2 == 0)
                    {
                        if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[2]/div/label/div/div[2]/div/input").Count() > 0)
                        {
                            d2++;
                        }
                    }
                    chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[2]/div/label/div/div[2]/div/input").SendKeys(txtPassTw.Text);
                    Thread.Sleep(1000);
                    for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                    {
                        if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                        {
                            dtgrvRegTw.Rows[i].Cells[2].Value = txtPassTw.Text;
                        }
                    }
                    //Ấn next
                    int d4 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count();
                    while (d4 == 0)
                    {
                        if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count() > 0)
                        {
                            d4++;
                        }
                    }
                    chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Click();
                    Thread.Sleep(3500);

                    if (chome.FindElementsByXPath("//span[contains(text(),'Welcome to Twitter!')]").Count() > 0)
                    {
                        for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                        {
                            if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                            {
                                dtgrvRegTw.Rows[i].Cells[4].Value = "Đã xong";
                            }
                        }

                    }
                    else
                    {
                        //Ấn Skip for now
                        int d3 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count();
                        while (d3 == 0)
                        {
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count() > 0)
                            {
                                d3++;
                            }
                        }
                        chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Click();
                        Thread.Sleep(2500);

                        //Ấn Skip for now
                        int d5 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count();
                        while (d5 == 0)
                        {
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Count() > 0)
                            {
                                d5++;
                            }
                        }
                        chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div").Click();
                        Thread.Sleep(4000);

                        //Chọn danh mục
                        if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[3]/section/div/div/div[3]/div/div/li[1]/div/div/div/div/div/div").Count() > 0)
                        {
                            chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[3]/section/div/div/div[3]/div/div/li[1]/div/div/div/div/div/div").Click();
                            Thread.Sleep(1000);
                            chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[3]/section/div/div/div[3]/div/div/li[2]/div/div/div/div/div/div").Click();
                            Thread.Sleep(1000);
                            chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[3]/section/div/div/div[3]/div/div/li[3]/div/div/div/div/div/div").Click();
                            Thread.Sleep(1000);
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div[2]").Count() > 0)
                            {
                                chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div/div[2]").Click();
                            }
                            else
                            {
                                chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Click(); //Next dài
                            }
                            Thread.Sleep(2500);
                        }
                        //Ấn next
                        int d7 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Count();
                        while (d7 == 0)
                        {
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Count() > 0)
                            {
                                d7++;
                            }
                        }
                        chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Click();
                        Thread.Sleep(2500);
                        //Ấn next
                        int d8 = chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Count();
                        while (d8 == 0)
                        {
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Count() > 0)
                            {
                                d8++;
                            }
                        }
                        chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Click();
                        Thread.Sleep(3500);

                        //Ấn skip
                        if (chome.FindElementsByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Count() > 0)
                        {
                            chome.FindElementByXPath("/html/body/div/div/div/div[1]/div[2]/div/div/div/div/div/div[2]/div[2]/div/div/div[2]/div[2]/div[2]/div").Click();
                            Thread.Sleep(2500);
                        }
                        //Lấy user
                        chome.FindElementByXPath("/html/body/div/div/div/div[2]/main/div/div/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div/div/div[1]/a").Click();
                        Thread.Sleep(2000);
                        string url = chome.Url;
                        user = url.Substring(20);
                        for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                        {
                            if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                            {
                                dtgrvRegTw.Rows[i].Cells[4].Value = "Đang Follow...";
                                dtgrvRegTw.Rows[i].Cells[5].Value = user;
                            }
                        }

                        //Đi follow
                        Thread.Sleep(1000);
                        chome.Navigate().GoToUrl("https://twitter.com/BotheredOtters");
                        Thread.Sleep(2000);
                        int d12 = chome.FindElementsByXPath("/html/body/div/div/div/div[2]/main/div/div/div/div[1]/div/div[2]/div/div/div[1]/div/div[1]/div/div[3]/div/div").Count();
                        while (d12 == 0)
                        {
                            if (chome.FindElementsByXPath("/html/body/div/div/div/div[2]/main/div/div/div/div[1]/div/div[2]/div/div/div[1]/div/div[1]/div/div[3]/div/div").Count() > 0)
                            {
                                d12++;
                            }
                        }
                        chome.FindElementByXPath("/html/body/div/div/div/div[2]/main/div/div/div/div[1]/div/div[2]/div/div/div[1]/div/div[1]/div/div[3]/div/div").Click();
                        Thread.Sleep(1000);
                        for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                        {
                            if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                            {
                                dtgrvRegTw.Rows[i].Cells[4].Value = "Đã xong!";
                            }
                        }
                        chome.Close();
                        chome.Quit();
                    }
                }
            }
            catch (Exception)
            {

                for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
                {
                    if (dtgrvRegTw.Rows[i].Cells[1].Value.ToString() == phone)
                    {
                        dtgrvRegTw.Rows[i].Cells[4].Value = "Lỗi";
                    }
                }
                Thread.Sleep(5000);
                chome.Quit();
            }
        }



        private void Seleniumm(int x1, int y1, string ipproxy, string userproxy, string passproxy, string useragent, string email, string passmail)
        {
            for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
            {
                if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                {
                    dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Đang reg...";
                }
            }
            bool checkip = false;

            ChromeDriver chome;
            ChromeOptions chromeOptions = new ChromeOptions();

            //Add Extention cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=650,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            try
            {
                //Kiểm tra xem có add extension không?
                if (checkip == true)
                {
                    ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                    string firstTab = windowHandles.First();
                    string lastTab = windowHandles.Last();
                    chome.SwitchTo().Window(firstTab);
                    chome.Close();
                    chome.SwitchTo().Window(lastTab);
                }

                //Thực hiện gán ip cho chrome
                Invoke(new Action(() =>
                {
                    if (!string.IsNullOrEmpty(ipproxy))
                    {
                        if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                        {
                            chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                            chome.Navigate();

                            chome.FindElementById("login").SendKeys(userproxy);
                            chome.FindElementById("password").SendKeys(passproxy);
                            chome.FindElementById("retry").Clear();
                            chome.FindElementById("retry").SendKeys("2");
                            chome.FindElementById("save").Click();
                        }
                    }
                }));

                //Vào việc
                //Login vào Outlook
                ((IJavaScriptExecutor)chome).ExecuteScript("window.open();");
                chome.SwitchTo().Window(chome.WindowHandles.Last());
                chome.Navigate().GoToUrl("https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&ct=1618573065&rver=7.0.6737.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26RpsCsrfState%3dc10a8031-c85b-da85-291b-635c48dc6392&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=90015");
                Thread.Sleep(1500);
                chome.FindElementById("i0116").SendKeys(email);
                Thread.Sleep(500);
                chome.FindElementById("idSIButton9").Click();
                Thread.Sleep(1500);
                chome.FindElementById("i0118").SendKeys(passmail);
                Thread.Sleep(500);
                chome.FindElementById("idSIButton9").Click();
                Thread.Sleep(2000);
                chome.FindElementById("idBtn_Back").Click();
                Thread.Sleep(1000);

                //Login vào tiktok
                ReadOnlyCollection<string> windowHandles1 = chome.WindowHandles;
                string firstTab1 = windowHandles1.First();
                string lastTab1 = windowHandles1.Last();
                chome.SwitchTo().Window(firstTab1);

                chome.Navigate().GoToUrl("https://ads.tiktok.com/i18n/createaccount?_source_=ads_homepage&redirect=https%3A%2F%2Fads.tiktok.com%2Fi18n%2Fhome%2F&country=VN");
                Thread.Sleep(TimeSpan.FromSeconds(2));

                //Kiểm tra cookie
                if (chome.FindElementsByLinkText("Cookies Policy").Count() > 0)
                {
                    chome.FindElementByXPath("//button[contains(text(),'Accept all')]").Click();
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }

                //Kiểm tra ip
                int rd1 = chome.FindElementsByClassName("vi-radio__inner").Count();
                if (rd1 != 0)
                {
                    chome.FindElementByClassName("vi-input__inner").Click();
                    Thread.Sleep(500);
                    chome.FindElementByClassName("vi-input__inner").SendKeys("Vie");
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body[1]/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[5]/div[1]/div[1]/div[2]/form[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/ul[1]/li[232]/span[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[5]/div[1]/div[1]/div[2]/form[1]/div[3]/div[1]/button[1]").Click();
                }

                //Bắt đầu đăng ký
                //Điền thông tin đăng ký
                int dem = chome.FindElementsByXPath("//span[contains(text(),'Sign up')]").Count();
                while (dem == 0)
                {
                    if (chome.FindElementsByXPath("//span[contains(text(),'Sign up')]").Count() > 0)
                    {
                        dem++;
                    }
                }
                //Điền email + pass
                chome.FindElementByXPath("//body/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[3]/div[1]/section[1]/form[1]/div[1]/div[1]/section[1]/div[2]/main[1]/form[1]/div[1]/div[2]/div[1]/div[1]/div[1]/input[1]").SendKeys(email);
                Thread.Sleep(500);
                chome.FindElementByXPath("//body/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[3]/div[1]/section[1]/form[1]/div[1]/div[1]/section[1]/div[2]/main[1]/form[1]/div[2]/div[1]/div[1]/div[1]/input[1]").SendKeys(txtPasss.Text);
                Thread.Sleep(500);
                //Điền Business Info
                string[] ArrBusiness = File.ReadAllLines(fnameBusiness);
                if (ArrBusiness.Length > 0)
                {
                    int abc = rd.Next(0, (ArrBusiness.Length) - 1);
                    chome.FindElementByXPath("//body/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[3]/div[1]/section[1]/form[1]/div[2]/div[1]/div[1]/input[1]").SendKeys(ArrBusiness[abc]);
                }
                else
                {
                    chome.FindElementByXPath("//body/section[1]/div[1]/section[1]/div[1]/div[1]/div[1]/section[1]/div[3]/div[1]/section[1]/form[1]/div[2]/div[1]/div[1]/input[1]").SendKeys("abc");
                }
                Thread.Sleep(500);
                //Điền Industry

                chome.FindElementByXPath("/html/body/section/div[1]/section/div/div/div[1]/section/div[3]/div/section/form/div[3]/div[1]/div/div/div[1]/input").Click();
                Thread.Sleep(500);
                int industrycount = rd.Next(0, 9);
                if (industrycount == 0)
                {
                    chome.FindElementByXPath("//span[contains(text(),'E-commerce')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'3C (Computers, Communications & Consumer)')]").Click();
                }
                else if (industrycount == 1)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Education & Training')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'College Education')]").Click();
                }
                else if (industrycount == 2)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Games')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Action & Shooting Games')]").Click();
                }
                else if (industrycount == 3)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Media & Content Creation')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Audio & Video Production')]").Click();
                }
                else if (industrycount == 4)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Offline Retail')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Alcoholic Beverages')]").Click();
                }
                else if (industrycount == 5)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Other')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Nonprofit & NGOs')]").Click();
                }
                else if (industrycount == 6)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Services')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Advertising & PR services')]").Click();
                }
                else if (industrycount == 7)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Sports & Entertainment')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Fitness')]").Click();
                }
                else if (industrycount == 8)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Utility Software')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Multimedia Processing')]").Click();
                }
                else if (industrycount == 9)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Sensitive Industries')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Animals')]").Click();
                }
                Thread.Sleep(500);
                //Điền số điện thoại
                int sodau = rd.Next(1000, 9999);
                int socuoi = rd.Next(1000, 9999);
                string phone = "09" + sodau.ToString() + socuoi.ToString();
                chome.FindElementByXPath("/html/body/section/div[1]/section/div/div/div[1]/section/div[3]/div/section/form/div[3]/div[3]/div/div/div/div/div/div/div/div/input").SendKeys(phone);
                Thread.Sleep(500);

                //Chọn loại tiền
                if (cbUSD.Checked)
                {
                    chome.FindElementByCssSelector(".input-content.vi-tooltip").Click();
                    Thread.Sleep(1000);
                    chome.FindElementByXPath("/html/body/section/div[1]/section/div/div/div[1]/section/div[3]/div/section/form/div[3]/div[4]/div[3]/div/div[2]/div[2]/div[2]/div/div/div/div[1]/input").Click();
                    Thread.Sleep(700);
                    chome.FindElementByXPath("//span[contains(text(),'USD')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Save')]").Click();
                    //Cập nhật startus
                    for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                    {
                        if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                        {
                            dataGridViewTaikhoanreg.Rows[i].Cells[5].Value = "USD";
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                    {
                        if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                        {
                            dataGridViewTaikhoanreg.Rows[i].Cells[5].Value = "VND";
                        }
                    }
                }
                //Check điều khoản
                chome.FindElementByClassName("vi-checkbox__inner").Click(); //Đợi reg
                Thread.Sleep(500);

                //Ấn sendcode
                chome.FindElementById("TikTokAds_Register-account-center-code-btn").Click();
                Thread.Sleep(500);

                //Cập nhật startus
                for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                {
                    if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dataGridViewTaikhoanreg.Rows[i].Cells[3].Value = txtPasss.Text;
                        dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Đợi capcha...";
                    }
                }

                //Điền info 2
                int dem1 = chome.FindElementsByClassName("vi-textarea__inner").Count();
                while (dem1 == 0)
                {
                    if (chome.FindElementsByClassName("vi-textarea__inner").Count() > 0)
                    {
                        dem1++;
                    }
                }

                for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                {
                    if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Đang điền info...";
                    }
                }
                Thread.Sleep(1000);
                chome.Navigate().GoToUrl("https://ads.tiktok.com/i18n/account/complete");
                Thread.Sleep(500);
                int dem2 = chome.FindElementsByClassName("vi-textarea__inner").Count();
                while (dem2 == 0)
                {
                    if (chome.FindElementsByClassName("vi-textarea__inner").Count() > 0)
                    {
                        dem2++;
                    }
                }
                //Điền Website
                string[] ArrWebsite = File.ReadAllLines(fnameWebsite);
                if (ArrWebsite.Length > 0)
                {
                    int indexweb = rd.Next(0, (ArrWebsite.Length) - 1);
                    chome.FindElementByClassName("vi-textarea__inner").SendKeys(ArrWebsite[indexweb]);
                }
                else
                {
                    chome.FindElementByClassName("vi-textarea__inner").SendKeys("https://totoshop.vn/");
                }
                Thread.Sleep(500);
                //Điền Address
                int state = rd.Next(0, 5);
                string add = "";
                chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[4]/div[1]/div[1]/div[1]/input[1]").Click();
                Thread.Sleep(300);
                if (state == 0)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Kien Giang Province')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Phu Quoc District')]").Click();
                    add = "Phu Quoc District";
                }
                else if (state == 1)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Quang Ngai Province')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Quang Ngai City')]").Click();
                    add = "Quang Ngai City";
                }
                else if (state == 2)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Can Tho City')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Ninh Kieu District')]").Click();
                    add = "Ninh Kieu District";
                }
                else if (state == 3)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Bac Ninh Province')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Tu Son Town')]").Click();
                    add = "Tu Son Town";
                }
                else if (state == 4)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Bac Kan Province')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Pac Nam District')]").Click();
                    add = "Pac Nam District";
                }
                else if (state == 5)
                {
                    chome.FindElementByXPath("//span[contains(text(),'Ben Tre Province')]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[5]/div[1]/div[1]/div[1]/input[1]").Click();
                    Thread.Sleep(500);
                    chome.FindElementByXPath("//span[contains(text(),'Thanh Phu District')]").Click();
                    add = "Thanh Phu District";
                }
                Thread.Sleep(500);
                chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[3]/div[1]/div[1]/input[1]").SendKeys(add);
                Thread.Sleep(500);
                //ĐIền PostlCode
                int code = rd.Next(100000, 999999);
                chome.FindElementByXPath("//body/div[@id='app']/section[1]/div[3]/section[1]/div[1]/div[1]/section[1]/div[2]/form[1]/div[2]/div[6]/div[1]/div[1]/input[1]").SendKeys(code.ToString());
                Thread.Sleep(500);
                //Chọn kiểu tài khoản
                int dem3 = chome.FindElementsByClassName("payment-icon").Count();
                if (dem3 == 2)
                {
                    if (rdbtnAuto.Checked)
                    {
                        chome.FindElementByXPath("//span[contains(text(),'Automatic Payment')]").Click();
                        for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                        {
                            if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                            {
                                dataGridViewTaikhoanreg.Rows[i].Cells[2].Value = "Automatic";
                            }
                        }

                    }
                    else if (rdbtnManual.Checked)
                    {
                        chome.FindElementByXPath("//span[contains(text(),'Manual Payment')]").Click();
                        for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                        {
                            if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                            {
                                dataGridViewTaikhoanreg.Rows[i].Cells[2].Value = "Manual";
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                    {
                        if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                        {
                            dataGridViewTaikhoanreg.Rows[i].Cells[2].Value = "Manual";
                        }
                    }
                }
                Thread.Sleep(500);
                //Ấn submit
                chome.FindElementByXPath("//span[contains(text(),'Submit')]").Click();
                Thread.Sleep(8000);

                //Check Payment lỗi
                bool checkpay = false;
                if (chome.FindElementsByCssSelector(".shark-payment-icon.svg-icon").Count() > 0)
                {
                    checkpay = true;
                }

                if (checkpay == true)
                {
                    for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                    {
                        if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                        {
                            dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Chưa xong!";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                    {
                        if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                        {
                            dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Đã xong!";
                        }
                    }
                }

                chome.Close();
                chome.Quit();
            }
            catch (Exception e)
            {
                for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
                {
                    if (dataGridViewTaikhoanreg.Rows[i].Cells[0].Value.ToString() == email)
                    {
                        dataGridViewTaikhoanreg.Rows[i].Cells[4].Value = "Lỗi!";
                    }
                }

                DateTime today = DateTime.Now;
                string filedate = today.ToString();
                filedate = filedate.Replace("/", "_");
                filedate = filedate.Replace(" ", "__");
                filedate = filedate.Replace(":", "_");
                string fileName = fnamelog + "Log" + filedate + ".txt";
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(e.Message);
                    fs.Write(title, 0, title.Length);
                }
                //chome.Close();
                chome.Quit();
            }
        }
        private void BackupAccount(string fname, DataGridView dgv)
        {
            TextWriter writer = new StreamWriter(fname);
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    if (c == 0)
                    {
                        writer.Write(dgv.Rows[r].Cells[c].Value.ToString());
                    }
                    else
                    {
                        writer.Write("|" + dgv.Rows[r].Cells[c].Value.ToString());
                    }

                }
                writer.WriteLine("");
            }
            writer.Close();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            richTextBoxResult.Text = "";
            for (int i = 0; i < dataGridViewTaikhoanreg.Rows.Count; i++)
            {
                for (int j = 0; j <= dataGridViewTaikhoanreg.Columns.Count - 1; j++)
                {
                    richTextBoxResult.Text = (richTextBoxResult.Text + dataGridViewTaikhoanreg.Rows[i].Cells[j].Value.ToString() + "|");

                }
                richTextBoxResult.Text = richTextBoxResult.Text.Remove(richTextBoxResult.Text.Length - 1, 1);
                richTextBoxResult.Text = richTextBoxResult.Text + "\n";
            }
        }



        private void ToExcel(DataGridView dGv, string filename)
        {
            string stOutput = "";
            string sHeaders = "";
            for (int j = 0; j < dGv.Columns.Count; j++)

                sHeaders = sHeaders.ToString() + Convert.ToString(dGv.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";

            for (int i = 0; i < dGv.RowCount - 1; i++)
            {
                string stline = "";
                for (int j = 0; j < dGv.Rows[i].Cells.Count; j++)
                    stline = stline.ToString() + Convert.ToString(dGv.Rows[i].Cells[j].Value) + "\t";
                stOutput += stline + "\r\n";


            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length);
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        private void rdbtnDungproxythuong_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnDungproxythuong.Checked)
            {
                groupBoxProxy.Visible = true;
            }
        }

        private void rdbtnKhongdungproxy_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnKhongdungproxy.Checked)
            {
                groupBoxProxy.Visible = false;
                groupBoxTinsoft.Visible = false;
            }
        }

        private void rdbtnDungproxytinsoft_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnDungproxytinsoft.Checked)
            {
                groupBoxProxy.Visible = true;
                groupBoxTinsoft.Visible = true;
            }
            else
            {
                groupBoxTinsoft.Visible = false;
            }
        }

        private void btnClearproxy_Click(object sender, EventArgs e)
        {
            richTextBoxProxy.Clear();
            richTextBoxProxy.Focus();
        }

        private void richTextBoxProxy_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameProxy, richTextBoxProxy.Text);
        }

        private void btnAddUa_Click(object sender, EventArgs e)
        {
            Process.Start(fnameUa);

            //string[] months = new string[12];
            //months[0] = "Jan";
            //months[1] = "Feb";
            //months[2] = "Mar";
            //months[3] = "Apr";
            //months[4] = "May";
            //months[5] = "Jun";
            //months[6] = "Jul";
            //months[7] = "Aug";
            //months[8] = "Sep";
            //months[9] = "Oct";
            //months[10] = "Nov";
            //months[11] = "2";
            //File.AppendAllLines(fnameUa, months);

        }

        private void btnClearUa_Click(object sender, EventArgs e)
        {
            File.Create(fnameUa).Close();
            MessageBox.Show("Đã xóa toàn bộ Useragent!", "Thông báo");
        }

        private void btnStartTinsoft_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            path = path + "\\Tinsoft";
            ProcessStartInfo startInfo = new ProcessStartInfo(path + "\\Tinsoft Proxy Driver.exe");
            startInfo.WorkingDirectory = path;
            //startInfo.Arguments = "hidden";
            Process.Start(startInfo);
        }

        private void btnChangeproxy_Click(object sender, EventArgs e)
        {
            string comman = "change_all";
            Tinsoft(comman);
        }
        private void Tinsoft(string cmm)
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect("127.0.0.1", 888);
            NetworkStream serverStream = clientSocket.GetStream();
            string command = cmm; // Các command có sẵn ở mục dưới

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(command);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[1024];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            richTextBoxLog.Text = returndata.Replace("\0", ""); // Giá trị trả về

        }

        private void btnHideserver_Click(object sender, EventArgs e)
        {
            string comman = "hide_form";
            Tinsoft(comman);
        }

        private void btnShowserver_Click(object sender, EventArgs e)
        {
            string cmmm = "show_form";
            Tinsoft(cmmm);
        }

        private void btnCheckautomanual_Click(object sender, EventArgs e)
        {
            string cmmm = "get_status";
            Tinsoft(cmmm);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            string cmmm = "start_auto";
            Tinsoft(cmmm);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            string cmmm = "stop_auto";
            Tinsoft(cmmm);
        }

        private void btnGetlistproxy_Click(object sender, EventArgs e)
        {
            string getlisstproxcy = "get_list_proxies";
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect("127.0.0.1", 888);
            NetworkStream serverStream = clientSocket.GetStream();
            string command = getlisstproxcy; // Các command có sẵn ở mục dưới

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(command);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[1024];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            returndata = returndata.Replace("\0", ""); // Giá trị trả về
            richTextBoxLog.Text = returndata;
            string kq = returndata.Replace("\"", "*");
            string[] arrKq = kq.Split('[');
            string[] arrKq2 = arrKq[1].Split(']');
            string[] arrKq3 = arrKq2[0].Split('*');

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrKq3.Length; i++)
            {
                if (i % 2 != 0)
                {
                    sb.AppendFormat("{0}", arrKq3[i]);
                    sb.AppendLine();
                }
            }
            richTextBoxProxy.Text = sb.ToString();
        }

        private void btnGetproxy_Click(object sender, EventArgs e)
        {
            string getproxy = "get_proxy";
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect("127.0.0.1", 888);
            NetworkStream serverStream = clientSocket.GetStream();
            string command = getproxy; // Các command có sẵn ở mục dưới

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(command);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[1024];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            returndata = returndata.Replace("\0", ""); // Giá trị trả về
            richTextBoxLog.Text = returndata;
            string kq = returndata.Replace("\"", "*");
            string[] arrKq = kq.Split('[');
            string[] arrKq2 = arrKq[1].Split(']');
            string[] arrKq3 = arrKq2[0].Split('*');

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrKq3.Length; i++)
            {
                if (i % 2 != 0)
                {
                    sb.AppendFormat("{0}", arrKq3[i]);
                    sb.AppendLine();
                }
            }
            richTextBoxProxy.Text = sb.ToString();
        }

        private void btnGetproxyrandom_Click(object sender, EventArgs e)
        {
            string getproxyradom = "get_proxy random";
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect("127.0.0.1", 888);
            NetworkStream serverStream = clientSocket.GetStream();
            string command = getproxyradom; // Các command có sẵn ở mục dưới

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(command);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[1024];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            returndata = returndata.Replace("\0", ""); // Giá trị trả về
            richTextBoxLog.Text = returndata;
            string kq = returndata.Replace("\"", "*");
            string[] arrKq = kq.Split('[');
            string[] arrKq2 = arrKq[1].Split(']');
            string[] arrKq3 = arrKq2[0].Split('*');

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrKq3.Length; i++)
            {
                if (i % 2 != 0)
                {
                    sb.AppendFormat("{0}", arrKq3[i]);
                    sb.AppendLine();
                }
            }
            richTextBoxProxy.Text = sb.ToString();
        }

        private void btnClearkey_Click(object sender, EventArgs e)
        {
            string clear = "clear_keys";
            Tinsoft(clear);
        }

        private void btnSetmintimeout_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDownSetmintimeout.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập giá trị Timeout!", "Thông báo");
            }
            else
            {
                string timeout = "set_min_timeout " + (int)numericUpDownSetmintimeout.Value;
                Tinsoft(timeout);
            }
        }

        private void btnSetmaxuse_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDownSetmaxuse.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập giá trị MaxUse!", "Thông báo");
            }
            else
            {
                string usermax = "set_max_use " + (int)numericUpDownSetmaxuse.Value;
                Tinsoft(usermax);
            }
        }

        private void btnSetthreadstop_Click(object sender, EventArgs e)
        {
            if (txtSetthreadstop.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Proxy cần dừng!", "Thông báo");
            }
            else
            {
                string stopproxy = "set_thread_stop " + txtSetthreadstop.Text;
                Tinsoft(stopproxy);
            }
            txtSetthreadstop.Clear();
        }

        private void btnHuongDan_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void btnAddkey_Click(object sender, EventArgs e)
        {
            //Add key
            string comman = "add_key " + txtKeyTinsoft.Text;
            Tinsoft(comman);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kq = "{*success*:true ,*message*:[*171.252.17.60:29088*]}";
            string[] arrKq = kq.Split('[');
            string[] arrKq2 = arrKq[1].Split(']');
            string[] arrKq3 = arrKq2[0].Split('*');

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrKq3.Length; i++)
            {
                if (i % 2 != 0)
                {
                    sb.AppendFormat("{0}", arrKq3[i]);
                    sb.AppendLine();
                }
            }
            richTextBoxProxy.Text = sb.ToString();
        }

        private void richTextBoxBusinName_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameBusiness, richTextBoxBusinName.Text);
        }

        private void richTextBoxWebsite_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameWebsite, richTextBoxWebsite.Text);
        }

        private void richTextBoxHotmail_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameHotmail, richTextBoxHotmail.Text);

            //Xóa hàng trong dataGridView
            while (dataGridViewTaikhoanreg.Rows.Count > 0)
            {
                dataGridViewTaikhoanreg.Rows.RemoveAt(0);
            }

            string[] lines = File.ReadAllLines(fnameHotmail);
            string[] values;
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                table.Rows.Add(row);
            }
        }

        private void rdbtnDungriengproxy_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnDungriengproxy.Checked)
            {
                string[] check = File.ReadAllLines(fnameProxy);
                numThread.Value = check.Length;
                numThread.Enabled = false;
            }
        }

        private void rdbtnDungchungproxy_CheckedChanged(object sender, EventArgs e)
        {
            numThread.Enabled = true;
            numThread.Value = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbUSD_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUSD.Checked)
            {
                cbVND.Checked = false;
            }
            else
            {
                cbVND.Checked = true;
            }
        }

        private void cbVND_CheckedChanged(object sender, EventArgs e)
        {
            if (cbVND.Checked)
            {
                cbUSD.Checked = false;
            }
            else
            {
                cbUSD.Checked = true;
            }
        }

        private void txtKeyTinsoft_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameKey, txtKeyTinsoft.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                btnCheckautomanual.Visible = true;
                btnAuto.Visible = true;
                btnManual.Visible = true;
                btnGetproxyrandom.Visible = true;
                btnClearkey.Visible = true;
                btnSetmintimeout.Visible = true;
                btnSetmaxuse.Visible = true;
                btnSetthreadstop.Visible = true;
                numericUpDownSetmintimeout.Visible = true;
                numericUpDownSetmaxuse.Visible = true;
                txtSetthreadstop.Visible = true;
            }
            else
            {
                btnCheckautomanual.Visible = false;
                btnAuto.Visible = false;
                btnManual.Visible = false;
                btnGetproxyrandom.Visible = false;
                btnClearkey.Visible = false;
                btnSetmintimeout.Visible = false;
                btnSetmaxuse.Visible = false;
                btnSetthreadstop.Visible = false;
                numericUpDownSetmintimeout.Visible = false;
                numericUpDownSetmaxuse.Visible = false;
                txtSetthreadstop.Visible = false;
            }
        }

        private void dataGridViewTaikhoanreg_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            BackupAccount(fnameAccount, dataGridViewTaikhoanreg);
        }

        private void btnMuahotmail_Click(object sender, EventArgs e)
        {
            if (txtAPIHotmail.Text != "")
            {
                label6.Text = "Đang mua, vui lòng chờ...";
                ChromeDriver chome;
                ChromeOptions chromeOptions = new ChromeOptions();
                //chromeOptions.AddArgument("--window-size=650,900");
                chromeOptions.AddExcludedArgument("enable-automation");
                chromeOptions.AddArgument("headless");
                chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
                chromeOptions.AddArguments("--disable-notifications"); // to disable notification
                chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
                //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
                //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;

                chome = new ChromeDriver(chromeDriverService, chromeOptions);
                chome.Manage().Window.Position = new Point(0, 0);
                int sl = (int)numberupdowSoHotmail.Value;

                if (rdbDongVan.Checked)
                {
                    string url = "http://dongvanfb.com/api/buyaccount.php?apiKey=" + txtAPIHotmail.Text + "&type=1&amount=" + sl;
                    chome.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    string kq = chome.PageSource;
                    chome.Quit();
                    kq = kq.Replace("\"", "*");

                    //Kiểm tra api
                    int checkapi = kq.LastIndexOf("Unauthorized");
                    if (checkapi == -1)
                    {
                        int a1 = kq.LastIndexOf("accounts*:*");
                        string hm = kq.Substring(a1 + 11);
                        int a2 = hm.LastIndexOf("*,*balance");
                        string hm2 = hm.Substring(0, a2);
                        string[] arrListStr = hm2.Split(';');
                        richTextBoxHotmail.Lines = arrListStr.ToArray();
                        label6.Text = "";
                        MessageBox.Show("Đã mua thành công!");
                    }
                    else
                    {
                        label6.Text = "";
                        MessageBox.Show("Vui lòng kiểm tra lại API!", "Lỗi!");
                    }
                }
                else
                {
                    string url = "http://api.maxclone.vn/api/portal/buyaccount?key=" + txtAPIHotmail.Text + "&type=HOTMAIL&quantity=" + sl;
                    chome.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    string kq = chome.PageSource;
                    chome.Quit();
                    kq = kq.Replace("\"", "*");

                    //Kiểm tra api
                    int checkapi = kq.LastIndexOf("Sai API key");
                    if (checkapi == -1)
                    {
                        int a1 = kq.LastIndexOf(":[");
                        string hm = kq.Substring(a1 + 2);
                        int a2 = hm.LastIndexOf("]}");
                        string hm2 = hm.Substring(0, a2);
                        hm2 = hm2.Replace("*,*", "|");
                        string[] arrListStr = hm2.Split(',');
                        for (int i = 0; i < arrListStr.Length; i++)
                        {
                            int a3 = arrListStr[i].LastIndexOf("{*Username*:*");
                            arrListStr[i] = arrListStr[i].Substring(a3 + 13);
                            int a4 = arrListStr[i].LastIndexOf("*}");
                            arrListStr[i] = arrListStr[i].Substring(0, a4);
                            arrListStr[i] = arrListStr[i].Replace("|Password*:*", "|");
                        }
                        richTextBoxHotmail.Lines = arrListStr.ToArray();
                        label6.Text = "";
                        MessageBox.Show("Đã mua thành công!");
                    }
                    else
                    {
                        label6.Text = "";
                        MessageBox.Show("Vui lòng kiểm tra lại API!", "Lỗi!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập API Key!", "Lỗi!");
            }

        }

        private void txtAPIHotmail_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameKeyApi, txtAPIHotmail.Text);
        }

        private void rtbUA_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameUa, rtbUA.Text);
        }

        private void rtbName_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameBusiness, richTextBoxBusinName.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ImprotAccount(tb2, dtgrvTaiKhoanTw, fnameTw);
        }

        private void rtbPhone_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnamePhone, rtbPhone.Text);

            //Xóa hàng trong dataGridView
            while (dtgrvRegTw.Rows.Count > 0)
            {
                dtgrvRegTw.Rows.RemoveAt(0);
            }

            string[] lines = File.ReadAllLines(fnamePhone);
            string[] values;
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                tb1.Rows.Add(row);
            }
        }

        private void btnStartTw_Click(object sender, EventArgs e)
        {
            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;
            string[] checkUa = File.ReadAllLines(fnameUa);
            int checkua = checkUa.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Phone = null;
            string Profile = null;
            int luong = (int)numericUpDownLuongtw.Value;
            int dem = 0;

            if (rdbtnKhongdungproxy.Checked)
            {
                int demip = -1;
                if (btnStart.Text == "Start")
                {
                    btnStart.Text = "Stop";
                    btnStart.BackColor = Color.FromArgb(255, 43, 42);
                    for (int i = 0; i < luong; i++)
                    {
                        var index = i;
                        int y = 900 * index;
                        for (int j = 0; j < 3; j++)
                        {
                            demip++;
                            var indexj = j;
                            int x = 650 * indexj;
                            dem++;
                            if (dem > luong)
                                break;

                            if (demip == dtgrvRegTw.Rows.Count)
                                break;

                            //Get UA
                            if (checkBoxKhongua.Checked)
                            {
                                Useragent = null;
                            }
                            else
                            {
                                if (cbxoaUasaukhidung.Checked)
                                {
                                    checkUa = File.ReadAllLines(fnameUa);
                                    checkua = checkUa.Length;
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = Removelissfilehotmai(indexua);
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                                else
                                {
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = checkUa[indexua];
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                            }

                            Phone = dtgrvRegTw.Rows[demip].Cells[1].Value.ToString();
                            Profile = "Tw" + Phone;
                            Thread t = new Thread(() =>
                            {
                                SeleniummTw(x, y, Phone, Ipproxy, Userproxy, Passproxy, Useragent, Profile);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.3));
                        }
                    }
                }
                else
                {
                    btnStart.Text = "Start";
                    btnStart.BackColor = Color.FromArgb(75, 201, 67);
                    try
                    {
                        Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                        foreach (Process processChrome in processesChrome)
                        {

                            processChrome.Kill();
                        }
                    }
                    catch { }
                }
            }
            else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
            {
                if (rdbtnDungriengproxy.Checked)
                {
                    int demip = -1;
                    if (btnStart.Text == "Start")
                    {
                        btnStart.Text = "Stop";
                        btnStart.BackColor = Color.FromArgb(255, 43, 42);
                        for (int i = 0; i < luong; i++)
                        {
                            var index = i;
                            int y = 900 * index;
                            for (int j = 0; j < 3; j++)
                            {
                                demip++;
                                var indexj = j;
                                int x = 650 * indexj;
                                dem++;
                                if (dem > luong)
                                    break;

                                string[] Allproxy = checkProxy[demip].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Phone = dtgrvRegTw.Rows[demip].Cells[1].Value.ToString();
                                Profile = "Tw" + Phone;
                                Thread t = new Thread(() =>
                                {
                                    SeleniummTw(x, y, Phone, Ipproxy, Userproxy, Passproxy, Useragent, Profile);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }

                    }
                    else
                    {
                        btnStart.Text = "Start";
                        btnStart.BackColor = Color.FromArgb(75, 201, 67);
                        try
                        {
                            Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                            foreach (Process processChrome in processesChrome)
                            {

                                processChrome.Kill();
                            }
                        }
                        catch { }
                    }
                }
                else if (rdbtnDungchungproxy.Checked)
                {
                    int demip = -1;
                    if (btnStart.Text == "Start")
                    {
                        btnStart.Text = "Stop";
                        btnStart.BackColor = Color.FromArgb(255, 43, 42);
                        for (int i = 0; i < luong; i++)
                        {
                            var index = i;
                            int y = 900 * index;
                            for (int j = 0; j < 3; j++)
                            {
                                demip++;
                                var indexj = j;
                                int x = 650 * indexj;
                                dem++;
                                if (dem > luong)
                                    break;
                                if (demip == dtgrvRegTw.Rows.Count)
                                    break;

                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Phone = dtgrvRegTw.Rows[demip].Cells[1].Value.ToString();
                                Profile = "Tw" + Phone;
                                Thread t = new Thread(() =>
                                {
                                    SeleniummTw(x, y, Phone, Ipproxy, Userproxy, Passproxy, Useragent, Profile);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }
                    }
                    else
                    {
                        btnStart.Text = "Start";
                        btnStart.BackColor = Color.FromArgb(75, 201, 67);
                        try
                        {
                            Process[] processesChrome = Process.GetProcessesByName("chromedriver");
                            foreach (Process processChrome in processesChrome)
                            {

                                processChrome.Kill();
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void btnMuaphone_Click(object sender, EventArgs e)
        {
            if (txtAPIPhone.Text != "")
            {
                label13.Text = "Đang mua, vui lòng chờ...";
                ChromeDriver chome;
                ChromeOptions chromeOptions = new ChromeOptions();
                //chromeOptions.AddArgument("--window-size=650,900");
                chromeOptions.AddExcludedArgument("enable-automation");
                //chromeOptions.AddArgument("headless");
                chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
                chromeOptions.AddArguments("--disable-notifications"); // to disable notification
                chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
                //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
                //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;

                chome = new ChromeDriver(chromeDriverService, chromeOptions);
                chome.Manage().Window.Position = new Point(0, 0);
                int sl = (int)nbupdPhone.Value;

                string[] arrListStr = new string[sl];
                for (int i = 0; i < sl; i++)
                {
                    chome.Navigate().GoToUrl("https://chothuesimcode.com/api?act=number&apik=" + txtAPIPhone.Text + "&appId=1030");
                    Thread.Sleep(2000);
                    string rs = chome.PageSource;

                    rs = rs.Replace("\"", "*");
                    //Tách sđt
                    string kq = "";
                    string id = "";
                    string ok = "";
                    string check = rs.Substring(0, 102);
                    string checkid = rs.Substring(0, 136);
                    int indexcheck = check.LastIndexOf("0");
                    if (indexcheck != -1)
                    {
                        id = checkid.Substring(128, 8);
                        kq = rs.Substring(147, 9);
                        ok = id + "|" + kq;

                    }
                    arrListStr[i] = ok;

                }
                chome.Quit();
                rtbPhone.Lines = arrListStr.ToArray();
                label13.Text = "";
                MessageBox.Show("Đã mua thành công!");

            }
            else
            {
                MessageBox.Show("Vui lòng nhập API Key!", "Lỗi!");
            }
        }



        private void ImprotAccount(DataTable tb, DataGridView drv, string fname)
        {
            StreamReader filest = new StreamReader(fname);
            string newline;
            while ((newline = filest.ReadLine()) != null)
            {
                DataRow dr = tb.NewRow();
                string[] values = newline.Split('|');
                for (int i = 1; i < values.Length; i++)
                {
                    dr[i] = values[i];
                }
                tb.Rows.Add(dr);
            }
            filest.Close();
            drv.DataSource = tb;
            for (int i = 0; i < drv.Rows.Count; i++)
            {
                drv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

        }
        private void ImprotAccountPs(DataTable tb, DataGridView drv, string fname)
        {
            StreamReader filest = new StreamReader(fname);
            string newline;
            while ((newline = filest.ReadLine()) != null)
            {
                DataRow dr = tb.NewRow();
                string[] values = newline.Split('|');
                for (int i = 0; i < values.Length - 1; i++)
                {
                    dr[i] = values[i];
                }
                tb.Rows.Add(dr);
            }
            filest.Close();
            drv.DataSource = tb;
            for (int i = 0; i < drv.Rows.Count; i++)
            {
                drv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

        }
        private void btnExportTw_Click(object sender, EventArgs e)
        {
            rtbExportTw.Text = "";
            for (int i = 0; i < dtgrvRegTw.Rows.Count; i++)
            {
                for (int j = 0; j <= dtgrvRegTw.Columns.Count - 1; j++)
                {
                    rtbExportTw.Text = (rtbExportTw.Text + dtgrvRegTw.Rows[i].Cells[j].Value.ToString() + "|");

                }
                rtbExportTw.Text = rtbExportTw.Text.Remove(rtbExportTw.Text.Length - 1, 1);
                rtbExportTw.Text = rtbExportTw.Text + "\n";
            }
        }

        private void rtbExportTw_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameTw, rtbExportTw.Text);
        }

        private void dtgrvTaiKhoanTw_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            BackupAccount(fnameBackupAccTW, dtgrvTaiKhoanTw);
        }

        private void btnViewChrome_Click(object sender, EventArgs e)
        {
            string useragnet = null;
            string nameprofile = null;

            //Kiểm tra xem có bao nhiêu ô được check => có bấy nhiêu luồng
            bool demcheck = false;
            for (int i = 0; i < dtgrvTaiKhoanTw.Rows.Count; i++)
            {
                object temp = dtgrvTaiKhoanTw.Rows[i].Cells[0].Value;
                if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                {
                    demcheck = true;

                }
            }
            if (demcheck == false)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần View in Chrome!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                int indexx = 0;
                int indexy = 0;
                int dempoint = 0;

                for (int i = 0; i < dtgrvTaiKhoanTw.Rows.Count; i++)
                {
                    object temp = dtgrvTaiKhoanTw.Rows[i].Cells[0].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        int x = 200 * indexx;
                        int y = 200 * indexy;
                        dempoint++;
                        if (dempoint == 3)
                        {
                            indexy++;
                            dempoint = 0;
                            indexx = 0;
                        }
                        else
                        {
                            indexx++;
                        }
                        if (dtgrvTaiKhoanTw.Rows.Count > 0)
                        {
                            nameprofile = dtgrvTaiKhoanTw.Rows[i].Cells[3].Value.ToString();
                            useragnet = dtgrvTaiKhoanTw.Rows[i].Cells[6].Value.ToString();
                        }
                        Thread t = new Thread(() =>
                        {
                            DriverViewinChrome(x, y, useragnet, nameprofile);
                        });
                        t.IsBackground = true;
                        t.Start();
                        Thread.Sleep(TimeSpan.FromSeconds(0.3));

                    }
                }
            }
        }

        private void DriverViewinChrome(int x1, int y1, string useragent, string nameProfile)
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            chromeOptions.AddExtension(ExtentionFolderPath + "\\extension_3_0_1_0.crx");
            chromeOptions.AddArgument("--window-size=900,700");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            chome.Navigate().GoToUrl("https://twitter.com/");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < dtgrvTaiKhoanTw.Rows.Count; j++)
            {
                for (int i = 0; i < dtgrvTaiKhoanTw.Rows.Count; i++)
                {
                    object temp = dtgrvTaiKhoanTw.Rows[i].Cells[0].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        dtgrvTaiKhoanTw.Rows.RemoveAt(i);

                    }
                }
            }
            for (int i = 0; i < dtgrvTaiKhoanTw.Rows.Count; i++)
            {
                dtgrvTaiKhoanTw.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            BackupAccount(fnameBackupAccTW, dtgrvRegTw);
        }

        private void btnMuahotmailPs_Click(object sender, EventArgs e)
        {
            if (txtAPIKeyPs.Text != "")
            {
                label14.Text = "Đang mua, vui lòng chờ...";
                ChromeDriver chome;
                ChromeOptions chromeOptions = new ChromeOptions();
                //chromeOptions.AddArgument("--window-size=650,900");
                chromeOptions.AddExcludedArgument("enable-automation");
                chromeOptions.AddArgument("headless");
                chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
                chromeOptions.AddArguments("--disable-notifications"); // to disable notification
                chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
                //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
                //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;

                chome = new ChromeDriver(chromeDriverService, chromeOptions);
                chome.Manage().Window.Position = new Point(0, 0);
                int sl = (int)nbudSLHotmailPs.Value;

                if (rdbDongVanPs.Checked)
                {
                    string url = "http://dongvanfb.com/api/buyaccount.php?apiKey=" + txtAPIKeyPs.Text + "&type=1&amount=" + sl;
                    chome.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    string kq = chome.PageSource;
                    chome.Quit();
                    kq = kq.Replace("\"", "*");

                    //Kiểm tra api
                    int checkapi = kq.LastIndexOf("Unauthorized");
                    if (checkapi == -1)
                    {
                        int a1 = kq.LastIndexOf("accounts*:*");
                        string hm = kq.Substring(a1 + 11);
                        int a2 = hm.LastIndexOf("*,*balance");
                        string hm2 = hm.Substring(0, a2);
                        string[] arrListStr = hm2.Split(';');
                        rtbHotmailPs.Lines = arrListStr.ToArray();
                        label14.Text = "";
                        MessageBox.Show("Đã mua thành công!");
                    }
                    else
                    {
                        label14.Text = "";
                        MessageBox.Show("Vui lòng kiểm tra lại API!", "Lỗi!");
                    }
                }
                else
                {
                    string url = "http://api.maxclone.vn/api/portal/buyaccount?key=" + txtAPIKeyPs.Text + "&type=HOTMAIL&quantity=" + sl;
                    chome.Navigate().GoToUrl(url);
                    Thread.Sleep(2000);
                    string kq = chome.PageSource;
                    chome.Quit();
                    kq = kq.Replace("\"", "*");

                    //Kiểm tra api
                    int checkapi = kq.LastIndexOf("Sai API key");
                    if (checkapi == -1)
                    {
                        int a1 = kq.LastIndexOf(":[");
                        string hm = kq.Substring(a1 + 2);
                        int a2 = hm.LastIndexOf("]}");
                        string hm2 = hm.Substring(0, a2);
                        hm2 = hm2.Replace("*,*", "|");
                        string[] arrListStr = hm2.Split(',');
                        for (int i = 0; i < arrListStr.Length; i++)
                        {
                            int a3 = arrListStr[i].LastIndexOf("{*Username*:*");
                            arrListStr[i] = arrListStr[i].Substring(a3 + 13);
                            int a4 = arrListStr[i].LastIndexOf("*}");
                            arrListStr[i] = arrListStr[i].Substring(0, a4);
                            arrListStr[i] = arrListStr[i].Replace("|Password*:*", "|");
                        }
                        rtbHotmailPs.Lines = arrListStr.ToArray();
                        label14.Text = "";
                        MessageBox.Show("Đã mua thành công!");
                    }
                    else
                    {
                        label14.Text = "";
                        MessageBox.Show("Vui lòng kiểm tra lại API!", "Lỗi!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập API Key!", "Lỗi!");
            }
        }

        private void rtbHotmailPs_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameHotmailPs, rtbHotmailPs.Text);

            //Xóa hàng trong dataGridView
            //while (dtgvTaiKhoanPs.Rows.Count > 0)
            //{
            //    dtgvTaiKhoanPs.Rows.RemoveAt(0);
            //}

            string[] lines = File.ReadAllLines(fnameHotmailPs);
            string[] values;
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                tb3.Rows.Add(row);
            }
        }

        private void dtgvTaiKhoanPs_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow dtr in dtgvTaiKhoanPs.Rows)
            {
                dtgvTaiKhoanPs.Rows[e.RowIndex].Cells[7].Value = e.RowIndex;
            }
            //BackupAccount(fnameAccPs, dtgvTaiKhoanPs);
        }

        private void dtgvTaiKhoanPs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            BackupAccount(fnameAccPs, dtgvTaiKhoanPs);
        }

        private void btnTaoPs_Click(object sender, EventArgs e)
        {
            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;
            string[] checkUa = File.ReadAllLines(fnameUa);
            int checkua = checkUa.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Email = null;
            string Profile = null;

            //Kiểm tra xem có bao nhiêu ô được check => có bấy nhiêu luồng
            bool demcheck = false;
            for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
            {
                object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                {
                    demcheck = true;

                }
            }
            if (demcheck == false)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                int indexx = 0;
                int indexy = 0;
                int dempoint = 0;

                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        if (rdbtnKhongdungproxy.Checked)
                        {
                            int x = 700 * indexx;
                            int y = 700 * indexy;
                            dempoint++;
                            if (dempoint == 3)
                            {
                                indexy++;
                                dempoint = 0;
                                indexx = 0;
                            }
                            else
                            {
                                indexx++;
                            }

                            //Get UA
                            if (checkBoxKhongua.Checked)
                            {
                                Useragent = null;
                            }
                            else
                            {
                                if (cbxoaUasaukhidung.Checked)
                                {
                                    checkUa = File.ReadAllLines(fnameUa);
                                    checkua = checkUa.Length;
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = Removelissfilehotmai(indexua);
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                                else
                                {
                                    if (checkua > 0)
                                    {
                                        int indexua = rd.Next(0, checkua - 1);
                                        Useragent = checkUa[indexua];
                                    }
                                    else
                                    {
                                        Useragent = null;
                                    }
                                }
                            }

                            Email = dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString();
                            Profile = "Pre" + dtgvTaiKhoanPs.Rows[i].Cells[7].Value.ToString();
                            Thread t = new Thread(() =>
                            {
                                SeleniumPs(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Profile);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.3));
                        }
                        else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
                        {
                            if (rdbtnDungriengproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[i].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Email = dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString();
                                Profile = "Pre" + dtgvTaiKhoanPs.Rows[i].Cells[7].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    SeleniumPs(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Profile);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                            else if (rdbtnDungchungproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                if (checkBoxKhongua.Checked)
                                {
                                    Useragent = null;
                                }
                                else
                                {
                                    if (cbxoaUasaukhidung.Checked)
                                    {
                                        checkUa = File.ReadAllLines(fnameUa);
                                        checkua = checkUa.Length;
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = Removelissfilehotmai(indexua);
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                    else
                                    {
                                        if (checkua > 0)
                                        {
                                            int indexua = rd.Next(0, checkua - 1);
                                            Useragent = checkUa[indexua];
                                        }
                                        else
                                        {
                                            Useragent = null;
                                        }
                                    }
                                }

                                Email = dtgvTaiKhoanPs.Rows[i].Cells[0].Value.ToString();
                                Profile = "Pre" + dtgvTaiKhoanPs.Rows[i].Cells[7].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    SeleniumPs(x, y, Ipproxy, Userproxy, Passproxy, Useragent, Email, Profile);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }
                    }
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameTukhoa, rtbTukhoa.Text);
        }

        private void btnviewchromepss_Click(object sender, EventArgs e)
        {
            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Profile = null;

            //Kiểm tra xem có bao nhiêu ô được check => có bấy nhiêu luồng
            bool demcheck = false;
            for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
            {
                object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                {
                    demcheck = true;

                }
            }
            if (demcheck == false)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                int indexx = 0;
                int indexy = 0;
                int dempoint = 0;

                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        if (rdbtnKhongdungproxy.Checked)
                        {
                            int x = 700 * indexx;
                            int y = 700 * indexy;
                            dempoint++;
                            if (dempoint == 3)
                            {
                                indexy++;
                                dempoint = 0;
                                indexx = 0;
                            }
                            else
                            {
                                indexx++;
                            }

                            //Get UA
                            Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                            Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                            Thread t = new Thread(() =>
                            {
                                DriverViewinChromePs(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.3));
                        }
                        else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
                        {
                            if (rdbtnDungriengproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[i].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverViewinChromePs(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                            else if (rdbtnDungchungproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverViewinChromePs(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }
                    }
                }
            }
        }

        private void DriverViewinChromePs(int x1, int y1, string useragent, string nameProfile, string ipproxy, string userproxy, string passproxy)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            bool checkip = false;

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            Invoke(new Action(() =>
            {
                //Add Extention cho chrome
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=800,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            //Tắt cửa sổ phụ hiện lên khi add extenion
            if (checkip == true)
            {
                ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                string firstTab = windowHandles.First();
                string lastTab = windowHandles.Last();
                chome.SwitchTo().Window(firstTab);
                chome.Close();
                chome.SwitchTo().Window(lastTab);
            }

            //Thực hiện gán ip cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy))
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                        chome.Navigate();

                        chome.FindElementById("login").SendKeys(userproxy);
                        chome.FindElementById("password").SendKeys(passproxy);
                        chome.FindElementById("retry").Clear();
                        chome.FindElementById("retry").SendKeys("2");
                        chome.FindElementById("save").Click();
                    }
                }
            }));

            //Vào việc
            chome.Navigate().GoToUrl("https://www.presearch.org/");

        }

        private void btnSearchPs_Click(object sender, EventArgs e)
        {
            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Profile = null;

            //Kiểm tra xem có bao nhiêu ô được check => có bấy nhiêu luồng
            bool demcheck = false;
            for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
            {
                object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                {
                    demcheck = true;

                }
            }
            if (demcheck == false)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                int indexx = 0;
                int indexy = 0;
                int dempoint = 0;

                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        if (rdbtnKhongdungproxy.Checked)
                        {
                            int x = 700 * indexx;
                            int y = 700 * indexy;
                            dempoint++;
                            if (dempoint == 3)
                            {
                                indexy++;
                                dempoint = 0;
                                indexx = 0;
                            }
                            else
                            {
                                indexx++;
                            }

                            //Get UA
                            Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                            Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                            Thread t = new Thread(() =>
                            {
                                DriverSearch(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.5));
                        }
                        else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
                        {
                            if (rdbtnDungriengproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[i].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverSearch(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                            }
                            else if (rdbtnDungchungproxy.Checked)
                            {
                                int x = 700 * indexx;
                                int y = 700 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverSearch(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                            }
                        }
                    }
                }
            }
        }

        private void DriverSearch(int x1, int y1, string useragent, string nameProfile, string ipproxy, string userproxy, string passproxy)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            bool checkip = false;

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            Invoke(new Action(() =>
            {
                //Add Extention cho chrome
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=800,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://www.presearch.org/");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            //Tắt cửa sổ phụ hiện lên khi add extenion
            if (checkip == true)
            {
                ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                string firstTab = windowHandles.First();
                string lastTab = windowHandles.Last();
                chome.SwitchTo().Window(firstTab);
                chome.Close();
                chome.SwitchTo().Window(lastTab);
            }

            //Thực hiện gán ip cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy))
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                        chome.Navigate();

                        chome.FindElementById("login").SendKeys(userproxy);
                        chome.FindElementById("password").SendKeys(passproxy);
                        chome.FindElementById("retry").Clear();
                        chome.FindElementById("retry").SendKeys("2");
                        chome.FindElementById("save").Click();
                    }
                }
            }));
            try
            {
                //Vào việc
                //Kiểm tra xem cần search bao nhiêu lần
                int solansr = 0;
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                    {
                        solansr = Int32.Parse(dtgvTaiKhoanPs.Rows[i].Cells[8].Value.ToString());
                    }
                }

                for (int j = 1; j <= solansr; j++)
                {
                    for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                    {
                        if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                        {
                            dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Search lần " + j;
                        }
                    }

                    chome.Navigate().GoToUrl("https://www.presearch.org/");
                    Thread.Sleep(2000);
                    int d0 = chome.FindElementsById("search").Count();
                    while (d0 == 0)
                    {
                        if (chome.FindElementsById("search").Count() > 0)
                        {
                            d0++;
                        }
                    }
                    //Random ra từ khóa tìm kiếm
                    string[] ArrBusiness = File.ReadAllLines(fnameTukhoa);
                    if (ArrBusiness.Length > 0)
                    {
                        int abc = rd.Next(0, (ArrBusiness.Length) - 1);
                        chome.FindElementById("search").SendKeys(ArrBusiness[abc]);
                    }
                    else
                    {
                        chome.FindElementById("search").SendKeys("Quan Ao");
                    }
                    Thread.Sleep(500);
                    //Click tìm kiếm
                    chome.FindElementByCssSelector(".rounded.btn.btn-lg.btn-default").Click();
                    Thread.Sleep(2000);
                    int d1 = chome.FindElementsByXPath("/html/body/div/div[2]/div[3]/div[2]/div[2]/div/div[3]/div/div[2]/div/div/div[1]/div[1]/a/span").Count();
                    while (d1 == 0)
                    {
                        if (chome.FindElementsByXPath("/html/body/div/div[2]/div[3]/div[2]/div[2]/div/div[3]/div/div[2]/div/div/div[1]/div[1]/a/span").Count() > 0)
                        {
                            d1++;
                        }
                    }
                    int index = rd.Next(2, 10);
                    chome.FindElementByXPath("/html/body/div/div[2]/div[3]/div[2]/div[2]/div/div[3]/div/div[" + index + "]/div/div/div[1]/div[1]/a/span").Click();

                    //Random khoảng time delay
                    int timesr = rd.Next((int)nbudtime1.Value, (int)nbudtime2.Value);
                    
                    Thread.Sleep(TimeSpan.FromSeconds(timesr));
                }
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Đã search xong!";
                    }
                }

                //Thoát chorme
                chome.Close();
                chome.Quit();
            }
            catch (Exception)
            {
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Search lỗi!";
                    }
                }
                chome.Quit();
            }
        }

        private void btnCheckCoin_Click(object sender, EventArgs e)
        {
            string[] checkProxy = File.ReadAllLines(fnameProxy);
            int checkproxy = checkProxy.Length;

            string Ipproxy = null;
            string Userproxy = null;
            string Passproxy = null;
            string Useragent = null;
            string Profile = null;

            //Kiểm tra xem có bao nhiêu ô được check => có bấy nhiêu luồng
            bool demcheck = false;
            for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
            {
                object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                {
                    demcheck = true;

                }
            }
            if (demcheck == false)
            {
                MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                int indexx = 0;
                int indexy = 0;
                int dempoint = 0;

                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    object temp = dtgvTaiKhoanPs.Rows[i].Cells[9].Value;
                    if (!Convert.IsDBNull(temp) && Convert.ToBoolean(temp))
                    {
                        if (rdbtnKhongdungproxy.Checked)
                        {
                            int x = 200 * indexx;
                            int y = 200 * indexy;
                            dempoint++;
                            if (dempoint == 3)
                            {
                                indexy++;
                                dempoint = 0;
                                indexx = 0;
                            }
                            else
                            {
                                indexx++;
                            }

                            //Get UA
                            Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                            Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                            Thread t = new Thread(() =>
                            {
                                DriverCheckCoin(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                            });
                            t.IsBackground = true;
                            t.Start();
                            Thread.Sleep(TimeSpan.FromSeconds(0.3));
                        }
                        else if (rdbtnDungproxythuong.Checked || rdbtnDungproxytinsoft.Checked)
                        {
                            if (rdbtnDungriengproxy.Checked)
                            {
                                int x = 200 * indexx;
                                int y = 200 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[i].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverCheckCoin(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                            else if (rdbtnDungchungproxy.Checked)
                            {
                                int x = 200 * indexx;
                                int y = 200 * indexy;
                                dempoint++;
                                if (dempoint == 3)
                                {
                                    indexy++;
                                    dempoint = 0;
                                    indexx = 0;
                                }
                                else
                                {
                                    indexx++;
                                }

                                //Get ip
                                string[] Allproxy = checkProxy[0].Split(':');
                                int countproxy = Allproxy.Length;
                                if (countproxy == 2)
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = null;
                                    Passproxy = null;
                                }
                                else
                                {
                                    Ipproxy = Allproxy[0] + ":" + Allproxy[1];
                                    Userproxy = Allproxy[2];
                                    Passproxy = Allproxy[3];
                                }

                                //Get UA
                                Useragent = dtgvTaiKhoanPs.Rows[i].Cells[6].Value.ToString();
                                Profile = dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString();
                                Thread t = new Thread(() =>
                                {
                                    DriverCheckCoin(x, y, Useragent, Profile, Ipproxy, Userproxy, Passproxy);
                                });
                                t.IsBackground = true;
                                t.Start();
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                        }
                    }
                }
            }
        }

        private void DriverCheckCoin(int x1, int y1, string useragent, string nameProfile, string ipproxy, string userproxy, string passproxy)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            bool checkip = false;

            string fileName = "Profile";
            FileInfo f = new FileInfo(fileName);
            string fullname = f.FullName;
            string ProfileFolderPath = fullname;
            //Tạo Profile
            //Kiểm tra xem có thư mục Profile chưa? Nếu chưa có thì tạo ra
            if (!Directory.Exists(ProfileFolderPath))
            {
                Directory.CreateDirectory(ProfileFolderPath);
            }

            //Nếu có thư mực Profile rồi thì tạo ra Profile
            if (Directory.Exists(ProfileFolderPath))
            {
                chromeOptions.AddArgument("user-data-dir=" + ProfileFolderPath + "\\" + nameProfile);
            }
            Invoke(new Action(() =>
            {
                //Add Extention cho chrome
                if (!string.IsNullOrEmpty(ipproxy)) //Kiểm tra xem ip có null hay không
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        checkip = true;
                        chromeOptions.AddExtension(ExtentionFolderPath + "\\ggmdpepbjljkkkdaklfihhngmmgmpggp-2.0-Crx4Chrome.com.crx");
                    }
                    chromeOptions.AddArgument(string.Format("--proxy-server={0}", ipproxy));
                }
            }));

            chromeOptions.AddArgument("--window-size=800,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument(string.Format("--user-agent={0}", useragent));
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeDriver chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(x1, y1);

            //Tắt cửa sổ phụ hiện lên khi add extenion
            if (checkip == true)
            {
                ReadOnlyCollection<string> windowHandles = chome.WindowHandles;
                string firstTab = windowHandles.First();
                string lastTab = windowHandles.Last();
                chome.SwitchTo().Window(firstTab);
                chome.Close();
                chome.SwitchTo().Window(lastTab);
            }

            //Thực hiện gán ip cho chrome
            Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(ipproxy))
                {
                    if (!string.IsNullOrEmpty(userproxy) && !string.IsNullOrEmpty(passproxy))
                    {
                        chome.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                        chome.Navigate();

                        chome.FindElementById("login").SendKeys(userproxy);
                        chome.FindElementById("password").SendKeys(passproxy);
                        chome.FindElementById("retry").Clear();
                        chome.FindElementById("retry").SendKeys("2");
                        chome.FindElementById("save").Click();
                    }
                }
            }));
            try
            {
                //Vào việc
                chome.Navigate().GoToUrl("https://www.presearch.org/");
                Thread.Sleep(2000);
                int d0 = chome.FindElementsByClassName("tour-balance").Count();
                while (d0 == 0)
                {
                    if (chome.FindElementsByClassName("tour-balance").Count() > 0)
                    {
                        d0++;
                    }
                }
                //Check coin
                // thực thi JavaScript dùng IJavaScriptExecutor
                IJavaScriptExecutor js = chome as IJavaScriptExecutor;
                // javascript cần return giá trị.
                var dataFromJS = (string)js.ExecuteScript("var content = document.getElementsByClassName('tour-balance')[0].children[0].innerHTML;return content;");
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[3].Value = dataFromJS;
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Check xong!";
                    }
                }
                Thread.Sleep(1000);
                //Thoát chorme
                chome.Close();
                chome.Quit();
            }
            catch (Exception)
            {
                for (int i = 0; i < dtgvTaiKhoanPs.Rows.Count; i++)
                {
                    if (dtgvTaiKhoanPs.Rows[i].Cells[5].Value.ToString() == nameProfile)
                    {
                        dtgvTaiKhoanPs.Rows[i].Cells[4].Value = "Check lỗi!";
                    }
                }
                //Thoát chorme
                chome.Close();
                chome.Quit();
            }

        }

        private void txtAPIKeyPs_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameKeyApi, txtAPIKeyPs.Text);
        }

        private void txtLinkgioithieu_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(fnameLinkgioithieu, txtLinkgioithieu.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtKeyTinsoft.Clear();
            txtKeyTinsoft.Focus();
            //Thêm cột tài khoản reg
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Passmail", typeof(string));
            table.Columns.Add("Payment", typeof(string));
            table.Columns.Add("Passtiktok", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("VNDUSD", typeof(string));
            dataGridViewTaikhoanreg.DataSource = table;

            //Đặt tên cột tài khoản reg
            dataGridViewTaikhoanreg.Columns[0].HeaderText = "Email";
            dataGridViewTaikhoanreg.Columns[1].HeaderText = "Pass mail";
            dataGridViewTaikhoanreg.Columns[2].HeaderText = "Payment";
            dataGridViewTaikhoanreg.Columns[3].HeaderText = "Pass tiktok";
            dataGridViewTaikhoanreg.Columns[4].HeaderText = "Status";
            dataGridViewTaikhoanreg.Columns[5].HeaderText = "VND/USD";

            //Thêm cột tw
            tb1.Columns.Add("ID", typeof(string));
            tb1.Columns.Add("EmailSDT", typeof(string));
            tb1.Columns.Add("Pass", typeof(string));
            tb1.Columns.Add("Profile", typeof(string));
            tb1.Columns.Add("Status", typeof(string));
            tb1.Columns.Add("User", typeof(string));
            tb1.Columns.Add("UA", typeof(string));
            dtgrvRegTw.DataSource = tb1;

            //Đặt tên cột tài khoản reg tw
            dtgrvRegTw.Columns[0].HeaderText = "ID";
            dtgrvRegTw.Columns[1].HeaderText = "Email/SĐT";
            dtgrvRegTw.Columns[2].HeaderText = "Pass Tw";
            dtgrvRegTw.Columns[3].HeaderText = "Profile";
            dtgrvRegTw.Columns[4].HeaderText = "Status";
            dtgrvRegTw.Columns[5].HeaderText = "User";
            dtgrvRegTw.Columns[6].HeaderText = "UA";

            //Thêm cột quản lý tài khoản tw
            tb2.Columns.Add("Check", typeof(Boolean));
            tb2.Columns.Add("Phone", typeof(string));
            tb2.Columns.Add("Pass", typeof(string));
            tb2.Columns.Add("Profile", typeof(string));
            tb2.Columns.Add("Status", typeof(string));
            tb2.Columns.Add("User", typeof(string));
            tb2.Columns.Add("UA", typeof(string));
            dtgrvTaiKhoanTw.DataSource = tb2;

            //Thêm cột quản lý tài khoản Ps
            tb3.Columns.Add("Email", typeof(string));
            tb3.Columns.Add("Passmail", typeof(string));
            tb3.Columns.Add("PassPre", typeof(string));
            tb3.Columns.Add("Coin", typeof(string));
            tb3.Columns.Add("Status", typeof(string));
            tb3.Columns.Add("Profile", typeof(string));
            tb3.Columns.Add("UserAgent", typeof(string));
            tb3.Columns.Add("Id", typeof(string));
            tb3.Columns.Add("LuotSearch", typeof(string));
            tb3.Columns.Add("Check", typeof(Boolean));
            dtgvTaiKhoanPs.DataSource = tb3;




            //Đặt lại tên cột Ps
            dtgvTaiKhoanPs.Columns[0].HeaderText = "Email";
            dtgvTaiKhoanPs.Columns[1].HeaderText = "Passmail";
            dtgvTaiKhoanPs.Columns[2].HeaderText = "PassPre";
            dtgvTaiKhoanPs.Columns[3].HeaderText = "Coin";
            dtgvTaiKhoanPs.Columns[4].HeaderText = "Status";
            dtgvTaiKhoanPs.Columns[5].HeaderText = "Profile";
            dtgvTaiKhoanPs.Columns[6].HeaderText = "UserAgent";
            dtgvTaiKhoanPs.Columns[7].HeaderText = "Id";
            dtgvTaiKhoanPs.Columns[8].HeaderText = "Lượt Search";
            dtgvTaiKhoanPs.Columns[9].HeaderText = "";



            ImprotAccount(tb2, dtgrvTaiKhoanTw, fnameBackupAccTW);

            ImprotAccountPs(tb3, dtgvTaiKhoanPs, fnameAccPs);
            //Ẩn
            groupBoxProxy.Visible = true;
            groupBoxTinsoft.Visible = false;

            if (fnameProxy.Length != 0)
            {
                richTextBoxProxy.Text = File.ReadAllText(fnameProxy);
            }
            if (fnameBusiness.Length != 0)
            {
                richTextBoxBusinName.Text = File.ReadAllText(fnameBusiness);
                rtbName.Text = File.ReadAllText(fnameBusiness);
            }
            if (fnameWebsite.Length != 0)
            {
                richTextBoxWebsite.Text = File.ReadAllText(fnameWebsite);
            }
            if (fnameTukhoa.Length != 0)
            {
                rtbTukhoa.Text = File.ReadAllText(fnameTukhoa);
            }
            if (fnameHotmail.Length != 0)
            {
                richTextBoxHotmail.Text = File.ReadAllText(fnameHotmail);
            }
            if (fnameKey.Length != 0)
            {
                txtKeyTinsoft.Text = File.ReadAllText(fnameKey);
            }

            if (fnameLinkgioithieu.Length != 0)
            {
                txtLinkgioithieu.Text = File.ReadAllText(fnameLinkgioithieu);
            }

            if (fnameKeyApi.Length != 0)
            {
                txtAPIKeyPs.Text = File.ReadAllText(fnameKeyApi);
            }

            if (fnameKeyApi.Length != 0)
            {
                txtAPIHotmail.Text = File.ReadAllText(fnameKeyApi);
            }
            if (fnameUa.Length != 0)
            {
                rtbUA.Text = File.ReadAllText(fnameUa);
            }

            //Ẩn chức năng Tinsoft
            btnCheckautomanual.Visible = false;
            btnAuto.Visible = false;
            btnManual.Visible = false;
            btnGetproxyrandom.Visible = false;
            btnClearkey.Visible = false;
            btnSetmintimeout.Visible = false;
            btnSetmaxuse.Visible = false;
            btnSetthreadstop.Visible = false;
            numericUpDownSetmintimeout.Visible = false;
            numericUpDownSetmaxuse.Visible = false;
            txtSetthreadstop.Visible = false;


        }

        private void btnGetTukhoa_Click(object sender, EventArgs e)
        {
            label23.Text = "Đang lấy dữ liệu, vui lòng chờ...";
            ChromeDriver chome;
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--window-size=650,900");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddArgument("headless");
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            //chromeOptions.AddArguments("--disable-extensions"); // to disable extension
            chromeOptions.AddArguments("--disable-notifications"); // to disable notification
            chromeOptions.AddArguments("--disable-application-cache");/* to disable cache*/
            //chromeOptions.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\Chrome.exe";
            //chromeOptions.AddArgument("--app=https://ads.tiktok.com/i18n/signup");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            chome = new ChromeDriver(chromeDriverService, chromeOptions);
            chome.Manage().Window.Position = new Point(0, 0);

            chome.Navigate().GoToUrl("https://trends.google.com.vn/trends/trendingsearches/realtime?geo=VN&category=all");
            Thread.Sleep(1000);
            int d0 = chome.FindElementsByClassName("content-header-title").Count();
            while (d0 == 0)
            {
                if (chome.FindElementsByClassName("content-header-title").Count() > 0)
                {
                    d0++;
                }
            }
            Thread.Sleep(3000);
            //Get từ khóa
            // thực thi JavaScript dùng IJavaScriptExecutor
            IJavaScriptExecutor js = chome as IJavaScriptExecutor;

            //Đếm xem có bao nhiêu từ khóa
            int d1 = chome.FindElementsByClassName("details-top").Count();
            if (d1 > 0)
            {
                string[] ArrTuKhoa = new string[d1];
                for (int i = 0; i < d1; i++)
                {
                    // javascript cần return giá trị.
                    var dataFromJS = (string)js.ExecuteScript("var content = document.getElementsByClassName('details-top')[" + i + "].children[0].innerHTML;return content;");
                    dataFromJS = dataFromJS.Replace("\"", "*");

                    int index = dataFromJS.IndexOf("Khám phá");
                    dataFromJS = dataFromJS.Substring(index);
                    int index1 = dataFromJS.IndexOf("*>\r\n");
                    dataFromJS = dataFromJS.Substring(0, index1);
                    int index2 = dataFromJS.IndexOf("Khám phá ");
                    dataFromJS = dataFromJS.Substring(index2 + 9);
                    ArrTuKhoa[i] = dataFromJS;
                }
                rtbTukhoa.Lines = ArrTuKhoa.ToArray();
                label23.Text = "";
                MessageBox.Show("Lấy dữ liệu thành công!");
            }
            else
            {
                MessageBox.Show("Lấy dữ liệu thất bại!\nVui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Thoát Chrome
            chome.Close();
            chome.Quit();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //Đặt lại kích thước cột
            dtgvTaiKhoanPs.Columns[0].Width = 40;
            dtgvTaiKhoanPs.Columns[1].Width = 40;
            dtgvTaiKhoanPs.Columns[2].Width = 40;
            dtgvTaiKhoanPs.Columns[3].Width = 30;
            dtgvTaiKhoanPs.Columns[5].Width = 30;
            dtgvTaiKhoanPs.Columns[6].Width = 50;
            dtgvTaiKhoanPs.Columns[7].Width = 20;
            dtgvTaiKhoanPs.Columns[8].Width = 40;
            dtgvTaiKhoanPs.Columns[9].Width = 20;
            button3.Visible = false;
        }
    }
}
