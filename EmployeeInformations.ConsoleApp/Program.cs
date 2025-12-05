//using EmployeeInformations.Model.EmployeesViewModel;
//using Newtonsoft.Json;
//using OfficeOpenXml;
//using System.Data;
using EmployeeInformations.Common.Helpers;
using System.Net;
using System.Net.Mail;

var mailFormat = EmailBodyContent.MailHeader() + EmailBodyContent.MailFooter();




//namespace TimeClock
//{
//    class Program
//    {
//        static void Main()
//        {
//            var blockStart = new TimeSpan(0, 6, 0, 0);
//            var blockEnd = new TimeSpan(0, 17, 0, 0);

//            var listOfTimeLogs = new List<TimeLog>
//            {
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,6,0,0),EntryType = EntryTypes.In},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,10,0,0),EntryType = EntryTypes.Out},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,10,15,0),EntryType = EntryTypes.In},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,12,0,0),EntryType = EntryTypes.Out},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,12,30,0),EntryType = EntryTypes.In},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,15,0,0),EntryType = EntryTypes.Out},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,15,15,0),EntryType = EntryTypes.In},
//                new TimeLog {EntryDateTime = new DateTime(2016,05,20,18,00,0),EntryType = EntryTypes.Out}
//            };


//            // You are going to have have for / for each unless you use Linq

//            // fist I would count clock in's versus the out's
//            var totalIn = listOfTimeLogs.Count(e => e.EntryType == EntryTypes.In);
//            var totalOut = listOfTimeLogs.Count() - totalIn;

//            // check if we have in the number of time entries
//            if (totalIn > totalOut)
//            {
//                Console.WriteLine("Employee didn't clock out");
//            }

//            // as I was coding this sample program, i thought of another way to store the time
//            // I would store them in blocks - we have to loop
//            var timeBlocks = new List<TimeBlock>();
//            for (var x = 0; x < listOfTimeLogs.Count; x += 2)
//            {
//                // create a new WORKING block based on the in/out time entries
//                timeBlocks.Add(new TimeBlock
//                {
//                    BlockType = BlockTypes.Working,
//                    In = listOfTimeLogs[x],
//                    Out = listOfTimeLogs[x + 1]
//                });

//                // create a BREAK block based on gaps
//                // check if the next entry in a clock in
//                var breakOut = x + 2;
//                if (breakOut < listOfTimeLogs.Count)
//                {
//                    var breakIn = x + 1;
//                    // create a new BREAK block
//                    timeBlocks.Add(new TimeBlock
//                    {
//                        BlockType = BlockTypes.Break,
//                        In = listOfTimeLogs[breakIn],
//                        Out = listOfTimeLogs[breakOut]
//                    });
//                }
//            }

//            var breakCount = 0;
//            // here is a loop for displaying detail
//            foreach (var block in timeBlocks)
//            {
//                var lineTitle = block.BlockType.ToString();
//                // this is me trying to be fancy
//                if (block.BlockType == BlockTypes.Break)
//                {
//                    if (block.IsBreak())
//                    {
//                        lineTitle = $"Break #{++breakCount}";
//                    }
//                    else
//                    {
//                        lineTitle = "Lunch";
//                    }
//                }
//                Console.WriteLine($" {lineTitle,-10} {block}  ===  Length: {block.Duration.ToString(@"hh\:mm")}");
//            }

//            // calculating total time for each block type
//            var workingTime = timeBlocks.Where(b => b.BlockType == BlockTypes.Working)
//                    .Aggregate(new TimeSpan(0), (p, v) => p.Add(v.Duration));

//            var breakTime = timeBlocks.Where(b => b.BlockType == BlockTypes.Break)
//                    .Aggregate(new TimeSpan(0), (p, v) => p.Add(v.Duration));


//            Console.WriteLine($"\nTotal Working Hours: {workingTime.ToString(@"hh\:mm")}");
//            Console.WriteLine($"   Total Break Time: {breakTime.ToString(@"hh\:mm")}");

//            Console.ReadLine();
//        }
//    }
//}
//string filePath = @"C:/Users/Vivek/Downloads/EmployeeHolidays.xlsx";
//FileInfo existingFile = new FileInfo(filePath);
//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//using (var excelPack = new ExcelPackage())
//{
//    var hasHeader = true;
//    //Load excel stream
//    using (var stream = File.OpenRead(filePath))
//    {
//        excelPack.Load(stream);
//    }

//    //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
//    var ws = excelPack.Workbook.Worksheets[0];

//    //Get all details as DataTable -because Datatable make life easy :)
//    DataTable excelasTable = new DataTable();
//    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
//    {
//        //Get colummn details
//        if (!string.IsNullOrEmpty(firstRowCell.Text))
//        {
//            string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
//            excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
//        }
//    }
//    var startRow = hasHeader ? 2 : 1;
//    //Get row details
//    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
//    {
//        var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
//        DataRow row = excelasTable.Rows.Add();
//        foreach (var cell in wsRow)
//        {
//            row[cell.Start.Column - 1] = cell.Text;
//        }
//    }
//    //Get everything as generics and let end user decides on casting to required type
//    var generatedType = JsonConvert.DeserializeObject<List<EmpHoliday>>(JsonConvert.SerializeObject(excelasTable));
//    //return (T)Convert.ChangeType(generatedType, typeof(T));
//    var empHolidays = new List<EmpHoliday>();
//    foreach(var item in generatedType)
//    {
//        var empHoliday = new EmpHoliday();
//        empHoliday.HolidayId = item.HolidayId;
//        empHoliday.Title = item.Title;
//        empHoliday.HolidayDate = item.HolidayDate;
//    }
//}

try
{
    var fromEmail = "noreply@vphospital.in";
    var host = "mail.vphospital.in";
    var port = Convert.ToInt32(465);

    MailMessage mailMessage = new MailMessage();

    var smtpServerAddress = host;
    var smtpUserName = fromEmail;
    var smtpPassword = "a4SCE6n63@";
    var fromAddress = fromEmail;
    var smtpPort = 8889;

    mailMessage.IsBodyHtml = true;
    MailAddress from = new MailAddress(fromAddress, "vphospital Support");
    mailMessage.From = from;
    mailMessage.To.Clear();
    mailMessage.To.Add(new MailAddress("vivek@vphospital.com"));

    mailMessage.Subject = "test";
    mailMessage.Body = "test";

    var client = new SmtpClient(smtpServerAddress, smtpPort)
    {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(smtpUserName, smtpPassword)

    };

    client.Send(mailMessage);

}
catch (SmtpException smptex)
{
    Convert.ToString(smptex.Message);
}
catch (Exception ex)
{
    Convert.ToString(ex.Message);

}



int[] myNum = { 10, 20 };

int[] numbers = { 1, 2, 3, 4, 5 };

string[] result = numbers.Select(i => i.ToString()).ToArray();
Console.WriteLine(String.Join(", ", result));

var str = "";
for (int i = 0; i < result.Count(); i++)
{
    var b = result[i];
    str += string.Format("'" + b + "',");
}
var aa = str.Remove(str.Length - 1, 1);

DateTime startDate = new DateTime(2022, 11, 6);
DateTime endDate = new DateTime(2022, 11, 6);

TimeSpan diff = endDate - startDate;
int days = diff.Days;
for (var i = 0; i <= days; i++)
{
    var testDate = startDate.AddDays(i);
    switch (testDate.DayOfWeek)
    {
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday:
            Console.WriteLine(testDate.ToShortDateString());
            break;
    }
}
Console.ReadLine();

var joinDate = Convert.ToDateTime("2022-11-05");
var currentDate = DateTime.Now;

var dayName = joinDate.DayOfWeek;

int monthCount = 0;
int casualLeave = 0;
int sickLeave = 0;
int earnLeave = 0;

var day = GetDaysInYear(joinDate);

monthCount = currentDate.Subtract(joinDate).Days / (day / 12);

if (monthCount <= 3)
{

}
else if (monthCount > 3)
{
    sickLeave += 1;
    casualLeave += 1;
}

if (monthCount > 12)
{
    earnLeave++;
}



static int GetDaysInYear(DateTime date)
{
    if (date.Equals(DateTime.MinValue))
    {
        return -1;
    }

    DateTime thisYear = new DateTime(date.Year, 1, 1);
    DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

    return (nextYear - thisYear).Days;
}
static void GetIpAddres()
{
    string hostName = Dns.GetHostName(); // Retrive the Name of HOST
    Console.WriteLine(hostName);
    // Get the IP
    var test = Dns.GetHostAddresses(hostName);
    string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
    Console.WriteLine("My IP Address is :" + myIP);
    Console.ReadKey();
}
