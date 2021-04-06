using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.Edge.SeleniumTools;

namespace Mono
{
    public partial class Mono : Form
    {
        static IWebDriver Browser;
        bool init = false;
        static DateTime checkTime, currentTime, robberyTime, tasksTime;
        static string url;
        bool fineLogin = true;
        byte characterClass = 0; //0 - гром, 1 - стрел, 2 - док
        bool side = true; //true - закон, false - братва
        bool gadget = false; //гаджет по типу ГП
        bool repair = true;
        bool robbery = true;
        bool tasks = true;
        bool bottles = true;
        bool lombard = true;
        bool dailymark = false;
        int cover = 0; //прикрытие
        int precCut = -1;

        static List<byte> diffList = new List<byte>();

        byte grayItem = 1; //{ "Оставить", "Продать" }
        byte specialItem = 2; //{ "Оставить", "Продать", "Общак" }
        String specialList;
        int timing = 400; //0 - по загрузке страницы, остальное с задержкой

        static List<string> taskList = new List<string>();
        static List<string> missions = new List<string> { "Бар \"Зелень\"", "Администрация Вокзала", "Усмирение тигра", "Заброшенный Завод", "Сочные дыни",
            "Беспредел в Порту", "Ограбление Века", "Особняк Фила Ричи", "Засада на Причале", "Мэр заказан", "Голубой Огонек",
            "Военная контрабанда", "Захват СМИ", "Автосалон Грида", "Склады Седого", "Последний полет", "Сорванная башня", 
            "Воскресный шоппинг", "Пляжная вечеринка", "Стрелка в парке" };
        static List<string> lomList = new List<string> {"Бостон", "Плазменные гранаты", "Голографический щит", "Чикаго", "Вирусные гранаты М2", "Медпакет PRO", "Вегас", "Истребитель", "КПК 1500МГц", "Жнец", "Джокер", 
            "Entry Gun", "Череп", "Отступник", "Мстител", "Скорпион", "Фрост", "Ящер", "Искател", "Убийц", "Silverballer", "Вышибал", "Спорт", "В законе", "Голиаф", "Подствольник ГП-30", "Штурмовой щит", "Вайпер", 
            "КПК 1000МГц", "Зажигательные патроны", "Ланцет", "Панацея", "Вирусные гранаты", "Разрушитель", "Подствольник ГП-25", "Снайпер", "Патроны с наведением", "Кислотник", "Пушка c ядами 50 калибр", "Скала", 
            "Армейский боевой щит", "Киборг", "КПК 750МГц", "Спасатель", "Армейский медпакет", "Фобос", "Террано", "Раптор", "Рептилия", "Питон", "Анаконда", "Репер", "Чика", "Фанат", "Улица", "Хардкор", "Киберпанк",
            "Хакер", "Ямай", "Урбан", "Модница", "Хипстер", "Подрывник", "Диверсант", "Призрак", "Фантом", "Костоправ", "Хирург", "Доминатор", "Вдова", "Предатор", "Кобра", "Кровавый", "Седого"};

        public Mono()
        {
            InitializeComponent();
            
            if (diffList.Count == 0) for (int i = 0; i < 20; i++) diffList.Add(0);
            //statusLabel.Text = "Статус: выбор настроек";

            ToolTip tipsForAll = new ToolTip();
            tipsForAll.AutoPopDelay = 5000;
            tipsForAll.InitialDelay = 800;
            tipsForAll.ReshowDelay = 500;
            tipsForAll.ShowAlways = true;

            List<string> browsers = new List<string> { "Chrome", "Firefox", "Edge" };
            browserSelect.Items.AddRange(browsers);
            browserSelect.SelectedIndex = 0; //окошко выбора браузера
            List<string> urls = new List<string> { "bratki.mobi", "spaces.im" };
            urlSelect.Items.AddRange(urls);
            urlSelect.SelectedIndex = 0;

            tipsForAll.SetToolTip(loginBox, "Логин");
            tipsForAll.SetToolTip(passwordBox, "Пароль");
            
            List<string> difficulties = new List<string> { "Нет", "Норма", "Профи", "Мастер" };
            diff1.Items.AddRange(difficulties);
            diff1.SelectedIndex = 3;
            diff2.Items.AddRange(difficulties);
            diff2.SelectedIndex = 3;
            diff3.Items.AddRange(difficulties);
            diff3.SelectedIndex = 3;
            diff4.Items.AddRange(difficulties);
            diff4.SelectedIndex = 3;
            diff5.Items.AddRange(difficulties);
            diff5.SelectedIndex = 3;
            diff6.Items.AddRange(difficulties);
            diff6.SelectedIndex = 3;
            diff7.Items.AddRange(difficulties);
            diff7.SelectedIndex = 3;
            diff8.Items.AddRange(difficulties);
            diff8.SelectedIndex = 3;
            diff9.Items.AddRange(difficulties);
            diff9.SelectedIndex = 3;
            diff10.Items.AddRange(difficulties);
            diff10.SelectedIndex = 3;
            diff11.Items.AddRange(difficulties);
            diff11.SelectedIndex = 3;
            diff12.Items.AddRange(difficulties);
            diff12.SelectedIndex = 1;
            diff13.Items.AddRange(difficulties);
            diff13.SelectedIndex = 1;
            diff14.Items.AddRange(difficulties);
            diff14.SelectedIndex = 1;
            diff15.Items.AddRange(difficulties);
            diff15.SelectedIndex = 1;
            diff16.Items.AddRange(difficulties);
            diff16.SelectedIndex = 1;
            diff17.Items.AddRange(difficulties);
            diff17.SelectedIndex = 1;
            diff18.Items.AddRange(difficulties);
            diff18.SelectedIndex = 1;
            diff19.Items.AddRange(difficulties);
            diff19.SelectedIndex = 1;
            diff20.Items.AddRange(difficulties);
            diff20.SelectedIndex = 0;

            List<string> gray = new List<string> { "Оставить", "Продать" };
            graySelect.Items.AddRange(gray);
            graySelect.SelectedIndex = 1;
            specialBox.Text = "Кредиты, Филки, Жетон, Пули, Снежок, Канистра с бензином, Айрон Скин, Капучино";

            List<string> special = new List<string> { "Оставить", "Продать", "Общак" };
            specialSelect.Items.AddRange(special);
            specialSelect.SelectedIndex = 2;

            timingLabel.Text = "Задержка: 600";
            stop.Enabled = false;
        }

        //fine
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) { if (Browser!=null) Browser.Quit(); }

        //fine
        private void BarScrolled(object sender, System.EventArgs e)
        {
            timingLabel.Text = "Задержка: " + timingBar.Value;
        }

        //fine
        private void stop_Click(object sender, EventArgs e)
        {
            BW.CancelAsync();
            init = false;
            if (Browser != null)
            {
                Browser.Quit();
                Browser = null;
            }
            taskList.Clear();
            robberyTime = DateTime.MinValue;
            tasksTime = DateTime.MinValue;
            start.Enabled = true;
            stop.Enabled = false;
        }

        //fine
        private void start_Click(object sender, EventArgs e)
        {
            InitPush();
            if (init) { 
                //statusLabel.Text = "Заполнение настроек";
                if (taskList.Count == 0)
                {
                    GetOptions();
                }
                BW.RunWorkerAsync();
                start.Enabled = false;
                stop.Enabled = true;
            }
            else
            {
                Browser.Quit();
            }
        }

        //fine
        private void InitPush() {
            if (!init)
            {
                fineLogin = true;
                checkTime = DateTime.UtcNow;
                IWebElement logElem;
                if (browserSelect.SelectedIndex == 0)
                {
                    Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                }
                else if (browserSelect.SelectedIndex == 1)
                {
                    Browser = new OpenQA.Selenium.Firefox.FirefoxDriver();
                }
                else if (browserSelect.SelectedIndex == 2)
                {
                    Browser = new EdgeDriver();
                }
                timing = timingBar.Value;
                if (timing <= 300) Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);
                Browser.Manage().Window.Position = new Point(0, 0);
                if (urlSelect.SelectedIndex == 0)
                {
                    url = "http://bratki.mobi";
                    Browser.Navigate().GoToUrl(url);
                    Thread.Sleep(300);
                    logElem = Browser.FindElement(By.ClassName("btn-login"));
                    logElem.Click();
                    Thread.Sleep(300);
                    logElem = Browser.FindElement(By.Id("login"));
                    logElem.SendKeys(loginBox.Text);
                    logElem = Browser.FindElement(By.Id("password"));
                    logElem.SendKeys(passwordBox.Text);
                    Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                    Thread.Sleep(300);
                    try
                    {
                        logElem = Browser.FindElement(By.ClassName("header-resources"));
                    }
                    catch (NoSuchElementException) { fineLogin = false; }
                    if (fineLogin)
                    {
                        //statusLabel.Text = "OK";
                        Thread.Sleep(400);
                        init = true;
                        Browser.Manage().Window.Size = new Size(520, 940);
                    }
                    else
                    {
                        Console.WriteLine("Статус: неправильный логин/пароль");
                        //statusLabel.Text = "Статус: неправильный логин/пароль";
                    }
                }
                else
                {
                    url = "http://bratva.spaces-games.com";
                    Browser.Navigate().GoToUrl("http://spaces.im");
                    Browser.Manage().Window.Size = new Size(920, 940);
                    Thread.Sleep(400);
                    Browser.FindElement(By.Name("contact")).SendKeys(loginBox.Text);
                    Browser.FindElement(By.Name("password")).SendKeys(passwordBox.Text);
                    logElem = Browser.FindElement(By.ClassName("btn_green"));
                    logElem.Click();
                    Thread.Sleep(300);
                    try
                    {
                        logElem = Browser.FindElement(By.ClassName("s_i_block-text"));
                    }
                    catch (NoSuchElementException) { fineLogin = false; }
                    if (fineLogin)
                    {
                        Browser.Navigate().GoToUrl("https://spaces.im/app/enter/?ent=12");
                        //statusLabel.Text = "OK";
                        Thread.Sleep(400);
                        init = true;
                        Browser.Manage().Window.Size = new Size(520, 940);
                    }
                    else
                    {
                        Console.WriteLine("Статус: неправильный логин/пароль");
                        //statusLabel.Text = "Статус: неправильный логин/пароль";
                    }
                }
            }
        }

        //нет доступа к интерфейсу у всего потока
        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!BW.CancellationPending)
            {
                if (init)
                {
                    TaskManager();
                }
            }
        }

        //нужно проверить работу с сутками, а так в порядке
        async private void TaskManager()
        {
            if (taskList.Count == 0)
            {
                SetTasks(dailymark);
                DealWithItems();
            }
            currentTime = DateTime.UtcNow;
            if ((currentTime - checkTime).TotalHours >= 24)
            {
                dailymark = false;
                checkTime = DateTime.UtcNow;
                SetTasks(dailymark);
            }
            
            while (taskList.Count!=0) {
                CheckOverfill();
                if (taskList[0].Contains("#robbery:")) DoRobbery();
                else if (taskList[0].Contains("#district:")) GoDistrict();
                else if (taskList[0].Contains("#tasks:")) DoTasks();
                else if (taskList[0].Contains("#missions:")) DoMissions();
                else if (taskList[0].Contains("#sleep:")) SetSleep();
                taskList.RemoveAt(0);
            }
            if (taskList.Count == 0) ResetTasks();
        }

        //must be fine 
        private void ResetTasks()
        {
            IWebElement elem;
            int mission_sec = -1, task_sec = -1, robbery_sec = -1, cut;
            bool done = false;
            
            if (tasks)
            {
                if(tasksTime.AddMinutes(cover) >= DateTime.UtcNow)
                {   
                    task_sec = (int)(tasksTime.AddMinutes(cover) - DateTime.UtcNow).TotalMilliseconds;
                }
                else
                {   //если восстановились Джереми и Тони "Фикса"
                    if (side) {
                        taskList.Add("#tasks: Джереми");
                        done = true;
                    }
                    else
                    {
                        taskList.Add("#tasks: Тони \"Фикса\"");
                        done = true;
                    }
                }
            }
            if (!done && robbery)
            {
                if (robberyTime.AddHours(2) >= DateTime.UtcNow)
                {
                    robbery_sec = (int)(robberyTime.AddHours(2) - DateTime.UtcNow).TotalMilliseconds;
                }
                else
                {   //если восстановлен патруль
                    taskList.Add("#robbery:");
                    done = true;
                }
            }
            if (!done) //заказы
            {
                int index = -1;
                bool tdone = false;
                Browser.Navigate().GoToUrl(url+"/missions");
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElement(By.XPath("//td[4]//a[1]")).Click();
                if (timing != 0) Thread.Sleep(timing);
                elem = Browser.FindElements(By.ClassName("list-btns"))[0];
                cut = elem.FindElements(By.TagName("a")).Count;
                for (int i=0; i < cut && !tdone; i++)
                {
                    index = missions.IndexOf(elem.FindElements(By.TagName("a"))[i].Text);
                    if (index != -1)
                    {
                        if (diffList[index] != 0)
                        {
                            taskList.Add("#missions: " + index);
                            tdone = true;
                        }
                    }
                }
                if (!tdone) {
                    string val, measure;
                    elem = Browser.FindElements(By.ClassName("list-btns"))[1];
                    cut = elem.FindElements(By.XPath("//div[@class='btn-flat no-arro inactive c-gray']")).Count;
                    for (int i=0; i < cut && !tdone; i++)
                    {
                        val = elem.FindElements(By.ClassName("btn-flat"))[i].Text;
                        for (int k = 0; k < missions.Count; k++)
                        {
                            if (val.Contains(missions[k]))
                            {
                                index = k;
                                break;
                            }
                        }
                        if (index != -1)
                        {
                            if (diffList[index] != 0)
                            {
                                measure = elem.FindElements(By.ClassName("btn-flat"))[i].FindElement(By.TagName("span")).Text;
                                val = measure.Remove(measure.IndexOf(" "), measure.Length - measure.IndexOf(" "));
                                if (measure.Contains("секунд")) mission_sec = int.Parse(val) * 1000;
                                else if (measure.Contains("минут")) mission_sec = int.Parse(val) * 60000;
                                else if (measure.Contains("час")) mission_sec = int.Parse(val) * 3600000;
                                tdone = true;
                            }
                        }
                    }
                }
                else { done = true; }
            }
            if (!done) //сон
            {
                if (robbery_sec == -1 && task_sec == -1 && mission_sec != -1) taskList.Add("#sleep: " + mission_sec);
                else if (robbery_sec == -1 && task_sec != -1 && mission_sec != -1)
                {
                    if(task_sec >= mission_sec) taskList.Add("#sleep: " + mission_sec);
                    else taskList.Add("#sleep: " + task_sec);
                }
                else if (robbery_sec != -1 && task_sec == -1 && mission_sec != -1)
                {
                    if(robbery_sec >= mission_sec) taskList.Add("#sleep: " + mission_sec);
                    else taskList.Add("#sleep: " + robbery_sec);
                }
                else if (robbery_sec != -1 && task_sec != -1 && mission_sec != -1)
                {
                    if(robbery_sec >= task_sec && mission_sec >= task_sec) taskList.Add("#sleep: " + task_sec);
                    else if (task_sec >= robbery_sec && mission_sec >= robbery_sec) taskList.Add("#sleep: " + robbery_sec);
                    else if (task_sec >= mission_sec && robbery_sec >= mission_sec) taskList.Add("#sleep: " + mission_sec);
                }
            }
            Console.WriteLine("Tasks: "+task_sec+"; Robbery: "+robbery_sec+"; Missions: "+mission_sec);
            //Console.WriteLine(taskList[0]);
        }

        //fine
        private void CheckOverfill()
        {
            IWebElement telem, ts;
            bool of = false;
            try
            {
                telem = Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("notice-inner"));
                try {
                    ts = telem.FindElements(By.TagName("div"))[0];
                    if (ts.Text == "В твоей сумке не хватает места!") of = true;
                    else Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                }
                catch (NoSuchElementException) {
                    try
                    {
                        Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                    }
                    catch (NoSuchElementException) { }
                }
                catch (ArgumentOutOfRangeException){
                    try
                    {
                        Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                    }
                    catch (NoSuchElementException) { }
                }
            }
            catch (NoSuchElementException) { }
            if (of) DealWithItems();
        }

        //тестировать
        private void DealWithItems() 
        {
            //statusLabel.Text = "Статус: очистка сумки";
            bool done = false, tdone = false, fine = true;
            string tempstr;
            IWebElement telem;
            int i = 0, lim = 0, k = 0, tlim = 0;
            try
            {
                Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
            }
            catch (NoSuchElementException) { }
            if (timing != 0) Thread.Sleep(timing);

            Browser.Navigate().GoToUrl(url + "/user/rack");
             Thread.Sleep(300);
            done = false;

            try
            {//кейсы
                while (!done)
                {
                    lim = Browser.FindElements(By.PartialLinkText("Открыть")).Count;
                    if (lim != 0)
                    {
                        lim = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr")).Count;

                        for (i = 0; i < lim; i++)
                        {
                            telem = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr"))[i];
                            tempstr = telem.FindElement(By.ClassName("font14")).FindElement(By.TagName("a")).Text;
                            if (tempstr == "Кейс" || tempstr == "Чемодан" || tempstr == "Дипломат")
                            {
                                done = false;
                                tlim = telem.FindElements(By.ClassName("btn-equip")).Count;
                                tdone = false;
                                for (k = 0; k < tlim && tdone == false; k++)
                                {
                                    try
                                    {
                                        if (telem.FindElements(By.ClassName("btn-equip"))[k].FindElement(By.TagName("span")).Text == "Открыть")
                                        {
                                            telem.FindElements(By.ClassName("btn-equip"))[k].Click();
                                            if (timing != 0) Thread.Sleep(timing);
                                            tdone = true;
                                            i = lim;
                                        }
                                    }
                                    catch (NoSuchElementException) { }
                                }
                            }
                        }
                    }
                    else done = true;
                }
            }
            catch (NoSuchElementException) { }
            

            if (specialItem==2) { //общак

                done = false;
                Browser.Navigate().GoToUrl(url + "/gang");
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElement(By.ClassName("list-btns")).FindElements(By.ClassName("btn-flat"))[2].Click();
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElements(By.ClassName("b-content"))[0].FindElement(By.ClassName("btn-a")).Click();
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElements(By.ClassName("b-content"))[1].FindElement(By.TagName("a")).Click();
                if (timing != 0) Thread.Sleep(timing);
            
                while (!done)
                {
                    try {
                        Browser.FindElements(By.TagName("table"))[1].FindElement(By.ClassName("btn-a")).Click();
                        if (timing != 0) Thread.Sleep(timing);
                    }
                    catch (NoSuchElementException) { done = true; }
                }
            }
            if (specialItem != 0 || grayItem != 0) {
                Browser.Navigate().GoToUrl(url + "/user/rack");
                if (timing != 0) Thread.Sleep(timing);
                done = false;

                while (!done)
                {
                    try
                    {
                        lim = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr")).Count;
                    }
                    catch (NoSuchElementException) { done = true; }
                    if (!done)
                    {
                        done = true;
                        for (i = 0; i < lim && done == true; i++)
                        {
                            telem = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr"))[i];
                            tempstr = telem.FindElement(By.ClassName("font14")).FindElement(By.TagName("a")).Text;
                            if (specialList.Contains(tempstr) == false)
                            {
                                try
                                {
                                    tempstr = telem.FindElement(By.XPath("//span[@class='font11 c-verygray']")).Text;
                                    if (tempstr == "личное")
                                    {
                                        if (grayItem == 1)
                                        {
                                            fine = true;
                                            if (bottles) {
                                                tempstr = telem.FindElement(By.ClassName("font14")).FindElement(By.TagName("a")).Text;
                                                if (tempstr.Contains("Настойка")) fine = false;
                                            }
                                            if (lombard) {
                                                tempstr = telem.FindElement(By.ClassName("font14")).FindElement(By.TagName("a")).Text;
                                                for (k = 0; k < lomList.Count; k++)
                                                    if (tempstr.Contains(lomList[k]))
                                                    {
                                                        fine = false;
                                                        break;
                                                    }
                                            }
                                            if (fine)
                                            {
                                                done = false;
                                                tlim = telem.FindElements(By.ClassName("btn-equip")).Count;
                                                tdone = false;
                                                for (k = 0; k < tlim && tdone == false; k++)
                                                {
                                                    try
                                                    {
                                                        telem.FindElement(By.PartialLinkText("Сбыть")).Click();
                                                        if (timing != 0) Thread.Sleep(timing);
                                                        Browser.FindElement(By.XPath("//a[@class='btn-a blue mt5']")).Click();
                                                        if (timing != 0) Thread.Sleep(timing);
                                                        tdone = true;
                                                    }
                                                    catch (NoSuchElementException) { }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (NoSuchElementException)
                                {
                                    if (specialItem == 1)
                                    {
                                        done = false;
                                        tlim = telem.FindElements(By.ClassName("btn-equip")).Count;
                                        tdone = false;
                                        for (k = 0; k < tlim && tdone == false; k++)
                                        {
                                            try
                                            {
                                                telem.FindElement(By.PartialLinkText("Сбыть")).Click();
                                                if (timing != 0) Thread.Sleep(timing);
                                                Browser.FindElement(By.XPath("//a[@class='btn-a blue mt5']")).Click();
                                                if (timing != 0) Thread.Sleep(timing);
                                                tdone = true;
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                }
                            }
                        }
                        if (i != lim) done = false;
                        else done = true;
                    }
                }
            }
        }

        //зависает тут
        private void DoRobbery()
        {
            bool check = true;
            IWebElement elem;
            //statusLabel.Text = "Статус: патрули/грабежи";
            Browser.Navigate().GoToUrl(url + "/robbery");
            try
            {
                elem = Browser.FindElement(By.XPath("a[@class='btn-a _robbery mt20']"));
                check = false;
            }
            catch (NoSuchElementException) { }
            if (check)
            {
                while (check)
                {
                    try
                    {
                        elem = Browser.FindElement(By.ClassName("btn-a"));
                        if (elem.GetAttribute("class").Contains("mt20"))
                        {
                            check = false;
                            robberyTime = DateTime.UtcNow;
                        }
                        else
                        {
                            elem.Click();
                            Thread.Sleep(500);
                        }
                    }
                    catch (NoSuchElementException) {
                        check = false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
            
        }

        //fine
        private void GoDistrict()
        {
            //statusLabel.Text = "Статус: смена района";
            String tmp = taskList[0];
            tmp = tmp.Replace("#district: ", "");
            Browser.Navigate().GoToUrl(url+"/skirmish/enter");
            if (timing != 0) Thread.Sleep(timing);
            if(Browser.FindElements(By.ClassName("subtitle"))[0].Text != tmp)
            {
                Browser.Navigate().GoToUrl(url + "/area/a3");
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElement(By.PartialLinkText("Посетить")).Click();
                if (timing != 0) Thread.Sleep(timing);
            }
        }

        //fine?
        private void DoTasks()
        {
            //statusLabel.Text = "Статус: наводки";
            if (taskList[0].Contains("everyday"))
            {
                Browser.Navigate().GoToUrl(url + "/everyday");
                if (timing != 0) Thread.Sleep(timing);
                try
                {
                    Browser.FindElement(By.ClassName("btn-a")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                }
                catch (NoSuchElementException) { }
            }
            else
            {
                String tmp = taskList[0].Replace("#tasks: ", ""), tval;
                bool fine = true, tf = false;
                IWebElement elem;
                int ti = 0, lim = 0, maxval = 0, maxpos = 0;
                if (tmp.Contains(',')) //любые комплексные задания
                {
                    Browser.Navigate().GoToUrl(url + "/tasks");
                    if (timing != 0) Thread.Sleep(timing);
                    if (!tmp.Contains(Browser.FindElement(By.ClassName("npc-name")).FindElement(By.TagName("span")).Text))
                    {
                        if (Browser.FindElements(By.ClassName("list-btns")).Count > 1) ti = 1;
                        lim = Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            if (tmp.Contains(Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].FindElement(By.TagName("span")).Text))
                            {
                                Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].Click();
                                fine = true;
                                break;
                            }
                            else
                            {
                                fine = false;
                            }
                        }
                        if (timing != 0) Thread.Sleep(timing);
                    }

                    if (fine)
                    {
                        try
                        {
                            Browser.FindElements(By.ClassName("list-btns"))[0].FindElement(By.ClassName("btn-flat")).Click();
                            if (timing != 0) Thread.Sleep(timing);
                        }
                        catch (NoSuchElementException) { }
                        
                        lim = Browser.FindElements(By.ClassName("b-content"))[0].FindElements(By.ClassName("btn-a")).Count;
                        for (int i = 0; i < lim && tf==false; i++)
                        {
                            tval = Browser.FindElements(By.ClassName("btn-a"))[i].FindElement(By.TagName("span")).Text;
                            if (tmp.Contains(tval))
                            {
                                Browser.FindElements(By.ClassName("btn-a"))[i].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElement(By.ClassName("btn-green")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                try
                                {
                                    elem = Browser.FindElement(By.ClassName("b-title"));
                                    if (elem.Text == "Задание выполнено!")
                                    {
                                        tf = true;
                                    }
                                }
                                catch (NoSuchElementException) {}
                                if (!tf)
                                {
                                    try
                                    {
                                        if (Browser.FindElement(By.ClassName("content-inner")).FindElements(By.ClassName("btn-a")).Count == 2)
                                        {
                                            tf = true;
                                        }
                                    }
                                    catch (NoSuchElementException){}
                                }
                                if (!tf)
                                {
                                    try
                                    {
                                        elem = Browser.FindElement(By.XPath("//span[@class='feedbackPanelERROR']"));
                                        if (elem.Text == "Ты не можешь сейчас проходить эту заказуху.")
                                        {
                                            tf = true;
                                            Browser.Navigate().GoToUrl(url + "/home");
                                            if (timing != 0) Thread.Sleep(timing);
                                            Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                                        }
                                    }
                                    catch (NoSuchElementException) {}
                                }
                                if (!tf)
                                {
                                    Fight(); tf = true;
                                }
                            }
                        }
                    }

                }
                else //Джереми и Тони "Фикса"
                {
                    Browser.Navigate().GoToUrl(url + "/tasks");
                    if (timing != 0) Thread.Sleep(timing);
                    if (Browser.FindElement(By.ClassName("npc-name")).FindElement(By.TagName("span")).Text != tmp)
                    {
                        if (Browser.FindElements(By.ClassName("list-btns")).Count > 1) ti = 1;
                        lim = Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            if (Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].FindElement(By.TagName("span")).Text == tmp)
                            {
                                Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].Click();
                                break;
                            }
                        }
                        if (timing != 0) Thread.Sleep(timing);
                    }
                    try
                    {
                        try
                        {
                            Browser.FindElement(By.ClassName("mt5")).FindElement(By.TagName("a")).Click();
                            if (timing != 0) Thread.Sleep(timing);
                        }
                        catch (NoSuchElementException) { }
                        lim = Browser.FindElement(By.ClassName("b-content")).FindElements(By.ClassName("btn-a")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            tval = Browser.FindElement(By.ClassName("b-content")).FindElements(By.ClassName("btn-a"))[i].FindElements(By.TagName("span"))[1].Text;
                            tval = tval.Replace("Прогресс: ", "");
                            tval = tval.Remove(tval.IndexOf('/'), tval.Length - tval.IndexOf('/'));
                            if (int.Parse(tval) > maxval)
                            {
                                maxval = int.Parse(tval); maxpos = i;
                            } 
                        }
                        Browser.FindElements(By.ClassName("btn-a"))[maxpos].Click();
                        if (timing != 0) Thread.Sleep(timing);
                        while (fine)
                        {
                            try
                            {
                                Browser.FindElement(By.ClassName("btn-green")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                            }
                            catch (NoSuchElementException) { fine = false; }
                        }
                        tasksTime = DateTime.UtcNow;
                    }
                    catch (NoSuchElementException) { fine = true; }

                }
            }
        }

        //fine
        private void DoMissions()
        {
            IWebElement elem;
            bool done = false;
            string tmp = taskList[0].Replace("#missions: ", "");
            byte t;
            //statusLabel.Text = "Статус: выбор заказухи";
            Browser.Navigate().GoToUrl(url + "/party");
            if (timing != 0) Thread.Sleep(timing);
            try { 
                elem = Browser.FindElement(By.ClassName("b-title")).FindElement(By.TagName("span"));
                //если команда уже есть - выход
                if (elem.Text == "Команда")
                {
                    Browser.FindElement(By.PartialLinkText("Покинуть команду")).Click();
                    Thread.Sleep(300);
                    Browser.FindElement(By.XPath("//a[@class='btn-a blue mt5']")).Click();
                    Thread.Sleep(300);
                    Browser.Navigate().GoToUrl(url + "/party");
                    Thread.Sleep(300);
                    elem = Browser.FindElement(By.ClassName("b-title")).FindElement(By.TagName("span"));
                }
                //если команда не создана
                if (elem.Text == "Создание команды")
                {
                    //если не выбраны заказухи
                    if (Browser.FindElement(By.ClassName("filter-el-active")).Text != "Заказухи")
                    {
                        Browser.FindElements(By.ClassName("filter-el"))[0].Click();
                        Thread.Sleep(300);
                    }
                    t = byte.Parse(tmp);
                    SelectElement sl = new SelectElement(Browser.FindElement(By.Name("partyName")));
                    sl.SelectByText(missions[t]);
                    if (timing != 0) Thread.Sleep(timing);
                    sl = new SelectElement(Browser.FindElement(By.Name("difficulty")));
                    sl.SelectByValue((diffList[t]-1).ToString());
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.Name("partyHidden")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.XPath("//input[@class='btn-a form-submit no-m font-normal']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    try
                    {   //привязь или низкий уровень               
                        elem = Browser.FindElement(By.XPath("//div[@class='list-btns mt5']//a[@class='btn-flat']"));
                    }
                    catch (NoSuchElementException) { 
                        done = true;
                    }
                    if (!done)
                    {   //удачно создана команда
                        elem.Click();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        Fight();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        CheckReward();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        DealWithItems();
                        done = true;
                    }
                }
            }
            catch (NoSuchElementException) { }
        }

        //слишком медленно - оптимизировать отлов исключений
        private void Fight()
        {
            bool done = false, action = false, abst, locked;
            IWebElement elem, telem, aelem;
            int holder;
            //statusLabel.Text = "Статус: бой";
            while (!done)
            {
                done = false;

                //проверка, есть ли бой
                try
                {
                    elem = Browser.FindElement(By.ClassName("b-title"));
                    if (elem.FindElement(By.TagName("span")).Text == "Задание выполнено!" || elem.Text == "Заказуха выполнена!") done = true;
                }
                catch (NoSuchElementException) { }
                if (!done)
                {
                    action = false;
                    try //боевые случаи
                    {
                        elem = Browser.FindElement(By.XPath("//table[@class='mt5']/tbody/tr"));
                        //ремонт
                        if (!action && repair)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("//a[@class='btn-a btn-repair50 no-m']"));
                                elem.Click();
                                if (timing != 0) Thread.Sleep(timing);
                                action = true;
                            }
                            catch (NoSuchElementException) { }
                        }
                        //гаджет
                        if (gadget && !action)
                        {
                            try
                            {
                                elem = Browser.FindElements(By.ClassName("btn-combat")).Last();
                                if (!elem.GetAttribute("class").Split(' ').Contains("btn-lock"))
                                {
                                    try
                                    {
                                        if (Browser.FindElements(By.ClassName("enemies-item")).Count != 0)
                                        {
                                            elem.Click();
                                            if (timing != 0) Thread.Sleep(timing);
                                            action = true;
                                        }
                                    }
                                    catch (NoSuchElementException) { }
                                }
                            }
                            catch (NoSuchElementException) { }
                        }
                        //способки
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("//table[@class='mt5']/tbody/tr"));
                                abst = false;
                                for (int i = 0; i < 4 && action == false && abst == false; i++)
                                {
                                    aelem = elem.FindElements(By.TagName("td"))[i];
                                    locked = aelem.FindElement(By.TagName("a")).GetAttribute("class").Split(' ').Contains("btn-lock");
                                    //точный выстрел
                                    if (aelem.FindElement(By.TagName("img")).GetAttribute("src").Contains("headshot"))
                                    {
                                        if (!locked)
                                        {
                                            try //обёрнут т.к. в кнопке может не быть ссылки
                                            {
                                                telem = Browser.FindElement(By.XPath("//td[@class='enemy-hp-amount']"));
                                                holder = int.Parse(telem.Text);
                                                if (holder >= precCut)
                                                {
                                                    aelem.FindElement(By.TagName("a")).Click();
                                                    if (timing != 0) Thread.Sleep(timing);
                                                    action = true; abst = true;
                                                }
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                    //первая помощь
                                    else if (!abst && aelem.FindElement(By.TagName("img")).GetAttribute("src").Contains("firstaid"))
                                    {
                                        if (!locked)
                                        {
                                            try
                                            {
                                                telem = Browser.FindElement(By.XPath("//a[@class='b-header small']/table[1]/tbody[1]/tr[1]/td[1]/span[1]"));
                                                if (telem.GetAttribute("class").Contains("warn"))
                                                {
                                                    aelem.FindElement(By.TagName("a")).Click();
                                                    if (timing != 0) Thread.Sleep(timing);
                                                    action = true; abst = true;
                                                }
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                    //яд
                                    else if (!abst && aelem.FindElement(By.TagName("img")).GetAttribute("src").Contains("poison"))
                                    {
                                        if (!locked)
                                        {
                                            try
                                            {
                                                telem = Browser.FindElement(By.ClassName("enemy-hp-amount"));
                                                aelem.FindElement(By.TagName("a")).Click();
                                                if (timing != 0) Thread.Sleep(timing);
                                                action = true; abst = true;
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                    
                                    //остальные способки
                                    else if (!abst)
                                    {
                                        if (!locked)
                                        {
                                            try
                                            {
                                                aelem.FindElement(By.TagName("a")).Click();
                                                if (timing != 0) Thread.Sleep(timing);
                                                action = true; abst = true;
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                }
                            }
                            catch (NoSuchElementException) { }
                        }
                        //обычный удар
                        if (!action)
                        {
                            try
                            {
                                Browser.FindElement(By.ClassName("btn-attack")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                            }
                            catch (NoSuchElementException) { }
                        }
                    }
                    catch (NoSuchElementException) //не боевые
                    {
                        //начало/продолжение заказа
                        try
                        {
                            elem = Browser.FindElement(By.XPath("//a[@class='btn-a bright t-c']"));
                            if (elem.Text.Contains("бой")) elem.Click();
                            if (timing != 0) Thread.Sleep(timing);
                            action = true;
                        }
                        catch (NoSuchElementException) { }
                        //завис
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("a[@class='btn-a t-c mt5']"));
                                if (elem.Text.Contains("Обновить страницу"))
                                {
                                    elem.Click();
                                    if (timing != 0) Thread.Sleep(timing);
                                    action = true;
                                }
                            }
                            catch (NoSuchElementException) { }
                        }
                        //лечение
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.ClassName("i-back"));
                                Browser.FindElement(By.ClassName("t-c")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                action = true;
                            }
                            catch (NoSuchElementException) { }
                        }

                    }
                }
            }
        }

        //не всегда срабатывает
        private void CheckReward() {
            IWebElement elem;
            //statusLabel.Text = "Статус: награда";
            try
            {
                elem = Browser.FindElement(By.XPath("//div[@class='b-panel-task']//td[2]"));
                if (elem.Text.Contains("выполнено"))
                {
                    elem.FindElement(By.XPath("//a[@class='btn-a btn-team btn-task']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.XPath("a[@class='btn-a t-c mb5']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                }
            }
            catch (NoSuchElementException) { }
        }

        //fine
        private void SetSleep()
        {
            //statusLabel.Text = "Статус: наелся и спит";
            Console.WriteLine("СОН");
            string tmp = taskList[0].Replace("#sleep: ", "");
            int ts = int.Parse(tmp), c = 0;
            Console.WriteLine("Продолжительность: "+ts);
            Browser.Navigate().GoToUrl(url+"/missions");
            while (c < ts)
            {
                Thread.Sleep(60000);
                c += 60000;
                Browser.Navigate().Refresh();
            }
        }

        //fine
        private void GetOptions()
        {
            String temp;
            if (gadgetCheck.Checked) gadget = true;
            else gadget = false;

            if (repairCheck.Checked) repair = true;
            else repair = false;

            if (robberyCheck.Checked) robbery = true;
            else robbery = false;

            if (tasksCheck.Checked) tasks = true;
            else tasks = false;

            grayItem = (byte) graySelect.SelectedIndex;
            specialList = specialBox.Text;
            specialItem = (byte) specialSelect.SelectedIndex;

            if (bottleCheck.Checked) bottles = true;
            else bottles = false;

            if (lombardCheck.Checked) lombard = true;
            else lombard = false;

            diffList[0] = (byte)diff1.SelectedIndex;
            diffList[1] = (byte)diff2.SelectedIndex;
            diffList[2] = (byte)diff3.SelectedIndex;
            diffList[3] = (byte)diff4.SelectedIndex;
            diffList[4] = (byte)diff5.SelectedIndex;
            diffList[5] = (byte)diff6.SelectedIndex;
            diffList[6] = (byte)diff7.SelectedIndex;
            diffList[7] = (byte)diff8.SelectedIndex;
            diffList[8] = (byte)diff9.SelectedIndex;
            diffList[9] = (byte)diff10.SelectedIndex;
            diffList[10] = (byte)diff11.SelectedIndex;
            diffList[11] = (byte)diff12.SelectedIndex;
            diffList[12] = (byte)diff13.SelectedIndex;
            diffList[13] = (byte)diff14.SelectedIndex;
            diffList[14] = (byte)diff15.SelectedIndex;
            diffList[15] = (byte)diff16.SelectedIndex;
            diffList[16] = (byte)diff17.SelectedIndex;
            diffList[17] = (byte)diff18.SelectedIndex;
            diffList[18] = (byte)diff19.SelectedIndex;
            diffList[19] = (byte)diff20.SelectedIndex;

            specialList = specialBox.Text;

            int.TryParse(precBox.Text, out precCut);
            if (precCut == -1) precCut = 10000;

            Browser.Navigate().GoToUrl(url + "/user");
            Thread.Sleep(300);
            temp = Browser.FindElements(By.ClassName("user-info"))[0].FindElements(By.TagName("span"))[0].Text;
            if (temp == "Законники") side = true;
            else side = false;
            temp = Browser.FindElements(By.ClassName("user-info"))[0].FindElements(By.TagName("span"))[1].Text;
            if (temp == "Громила") characterClass = 0;
            else if (temp == "Стрелок") characterClass = 1;
            else if (temp == "Доктор") characterClass = 2;
            Browser.Navigate().GoToUrl(url + "/user/cover");
            Thread.Sleep(300);
            temp = Browser.FindElements(By.ClassName("content-inner"))[0].FindElements(By.TagName("span"))[1].Text;
            while (temp.Contains('/')) temp = temp.Remove(0, 1);
            cover = int.Parse(temp);
        }

        //доделать наводки "Агент Гурин" братва
        private void SetTasks(bool daily) { 
            //statusLabel.Text = "Статус: составление маршрута";
            if (robbery)
            {
                if(robberyTime == DateTime.MinValue) taskList.Add("#robbery:");
                else if ((checkTime - robberyTime).TotalHours >= 2.0) taskList.Add("#robbery:");
            }
            
            if (tasks)
            {
                taskList.Add("#district: Порт Луи");
                if(daily == false) taskList.Add("#tasks: everyday");
                if (side)
                {
                    if (tasksTime == DateTime.MinValue) taskList.Add("#tasks: Джереми");
                    else if ((checkTime - tasksTime).TotalMinutes >= cover) taskList.Add("#tasks: Джереми");
                    if (daily == false) {
                        taskList.Add("#tasks: Зам. начальника, Всем по заслугам");
                        taskList.Add("#tasks: Зам. начальника, Футбол - Порт Луи");
                        taskList.Add("#tasks: Зам. начальника, Футбол - Площадь культуры.");
                        taskList.Add("#tasks: Зам. начальника, Футбол - Старый город");
                        taskList.Add("#tasks: Коля \"Молот\", Разгром: Борьба с законом");
                        taskList.Add("#tasks: Коля \"Молот\", Разгром: Налет");
                        taskList.Add("#tasks: Коля \"Молот\", Разгром: Налет");
                        taskList.Add("#tasks: Коля \"Молот\", Разгром: Борьба за территорию");
                        taskList.Add("#tasks: Саня \"Эксперт\", Большие проблемы");
                        taskList.Add("#tasks: Саня \"Эксперт\", Сезон охоты");
                        taskList.Add("#tasks: Саня \"Эксперт\", Черная метка");
                        taskList.Add("#tasks: Агент Гурин, Безумие: Старец");
                        taskList.Add("#tasks: Агент Гурин, Безумие: Наркобарон");
                        taskList.Add("#tasks: Агент Гурин, Безумие: Циркач");
                        taskList.Add("#tasks: Агент Гурин, Безумие: Тут вам не Мексика");
                        taskList.Add("#tasks: Агент Гурин, Безумие в Горках");
                        taskList.Add("#tasks: Агент Гурин, Безумие: Шрам");
                    }
                    
                }
                else
                {
                    if (tasksTime == DateTime.MinValue) taskList.Add("#tasks: Тони \"Фикса\"");
                    else if ((checkTime - tasksTime).TotalMinutes >= cover) taskList.Add("#tasks: Тони \"Фикса\"");

                    if (daily == false)
                    {
                        taskList.Add("#tasks: Пахан, Всем по заслугам");
                        taskList.Add("#tasks: Пахан, Футбол - Порт Луи");
                        taskList.Add("#tasks: Пахан, Футбол - Площадь культуры.");
                        taskList.Add("#tasks: Пахан, Футбол - Старый город");
                        taskList.Add("#tasks: \"Вася Тощий\", Разгром: Борьба с законом");
                        taskList.Add("#tasks: \"Вася Тощий\", Разгром: Налет");
                        taskList.Add("#tasks: \"Вася Тощий\", Разгром: Налет");
                        taskList.Add("#tasks: \"Вася Тощий\", Разгром: Борьба за территорию");
                        taskList.Add("#tasks: Саня \"Эксперт\", Большие проблемы");
                        taskList.Add("#tasks: Саня \"Эксперт\", Сезон охоты");
                        taskList.Add("#tasks: Саня \"Эксперт\", Черная метка");
                        //taskList.Add("#tasks: Агент Гурин, Безумие: Старец");
                        //taskList.Add("#tasks: Агент Гурин, Безумие: Наркобарон");
                        //taskList.Add("#tasks: Агент Гурин, Безумие: Циркач");
                        //taskList.Add("#tasks: Агент Гурин, Безумие: Тут вам не Мексика");
                        //taskList.Add("#tasks: Агент Гурин, Безумие в Горках");
                        //taskList.Add("#tasks: Агент Гурин, Безумие: Шрам");
                    }
                }
            }
            for (int i = 0; i < 5; i++) if (diffList[i] != 0) taskList.Add("#missions: " + i);
            if (diffList[5] != 0)
            {
                if (!daily && tasks) taskList.Add("#tasks: Саня \"Эксперт\", Беспредел в порту");
                taskList.Add("#missions: 5");
            }
            if (diffList[6] != 0)
            {
                if (!daily && tasks) taskList.Add("#tasks: Саня \"Эксперт\", Ограбление века");
                taskList.Add("#missions: 6");
            }
            if (diffList[7] != 0)
            {
                if (!daily && tasks) taskList.Add("#tasks: Саня \"Эксперт\", Богатенький Ричи");
                taskList.Add("#missions: 7");
            }
            if (diffList[8] != 0)
            {
                if (!daily && tasks) taskList.Add("#tasks: Саня \"Эксперт\", Засада на причале");
                taskList.Add("#missions: 8");
            }
            if (diffList[9] != 0)
            {
                if (!daily && tasks) taskList.Add("#tasks: Саня \"Эксперт\", Мэр Заказан");
                taskList.Add("#missions: 9");
            }
            for (int i = 10; i < 20; i++) if (diffList[i] != 0) taskList.Add("#missions: " + i);
        }

    }
}
