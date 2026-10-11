/***********************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
************************/
using System;
using System.Runtime.Versioning;
namespace System;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Text.Encoding.UTF8;
        // Khởi tạo các đổi tượng
        Computer comp1 = new Computer("CPT001","Máy trạm thực hành", 2024, 24500000m, DeviceStatus.Active, 16.0f, "Intel Core i7-13620", false);
        Computer comp2 = new Computer("CPT002", "Máy trạm đồ họa", 2022, 25000000m, DeviceStatus.Active, 32.0f, "AMD Ryzen 7 5800X", true);

        Printer prt1 = new Printer("PRT001", "Máy in Canon LBP2900", 2021, 3500000m, DeviceStatus.UnderMaintenance, "Inkjet", 17000, false);
        NetworkPrinter prt2 = new NetworkPrinter("PRT002", "Máy in mạng HP Color LaserJet Pro", 2025, 18000000m, DeviceStatus.Active, "Laser", 125000, true);

        Projector prj1 = new Projector("PRJ001", "Máy chiếu Sony VPL-EX455", 2020, 14000000m, DeviceStatus.Active, 3600, 3500);

        LabRoom lr1 = new LabRoom("LR001", "Phòng Thực hành Mạng & Đồ họa", 40);
        LabRoom lr2 = new LabRoom("LR002", "Phòng Thực hành Lập trình Cơ bản", 35);

        // 1. Thêm các thiết bị vào phòng
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("1. Thêm các thiết bị vào phòng");
        lr1.AddDevice(comp1);
        lr1.AddDevice(comp2);
        lr1.AddDevice(prt2);
        lr1.AddDevice(prj1);

        lr2.AddDevice(prt1);

        // 2. Thêm 1 thiết bị trùng mã
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("2. Thêm 1 thiết bị trùng mã");
        Computer cpt3 = new Computer("CPT001", "Máy tính giả lập trùng mã", 2026, 10000000m, DeviceStatus.Retired, 8.0f, "Intel Core i3-12100", false);
        try
        {
            lr1.AddDevice(cpt3);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Lỗi! {ex.Message}");
        }

        // 3. In danh sách thiết bị trong từng phòng
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("3. In danh sách thiết bị trong từng phòng");
        lr1.DisplayDevicesList();
        lr2.DisplayDevicesList();

        // 4. Tính tổng chi phí bảo trì dự kiến của mỗi phòng
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("4. Tính tổng chi phí bảo trì dự kiến của mỗi phòng");
        Console.WriteLine("Chi phí bảo trì của phòng LR0001: " + $"{lr1.CalculateAnnualMaintenanceCost()}");
        Console.WriteLine("Chi phí bảo trì của phòng LR0002: " + $"{lr2.CalculateAnnualMaintenanceCost()}");
        

        // 5. Liệt kê thiết bị cần bảo trì
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("5. Liệt kê thiết bị cần bảo trì");
        Console.WriteLine("Các thiết bị cần bảo trì phòng LR001:");
        List<Device> lr1MaintenanceDevices = lr1.GetDevicesRequiringMaintenance();
        foreach (var d in lr1MaintenanceDevices)
        {
            Console.WriteLine($"    {d.GetDeviceID}");
        }

        Console.WriteLine("Các thiết bị cần bảo trì phòng LR002:");
        List<Device> lr2MaintenanceDevices = lr2.GetDevicesRequiringMaintenance();
        foreach (var d in lr2MaintenanceDevices)
        {
            Console.WriteLine($"    {d.GetDeviceID}");
        }        



        // 6. Kết nối mạng cho các đối tượng thực thi INetworkable
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("6. Kết nối mạng cho các đối tượng thực thi INetworkable");
        
            // Máy tính 1 kết nối bình thường
        comp1.Connect("192.168.1.10");

            // Máy tính 2 kết nối địa chỉ rỗng
        try
        {
            comp2.Connect("");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Lỗi, {ex.Message}");
        }

            // Máy in 2 kết nối tới địa chỉ khác khi vẫn còn kết nối cũ
        prt2.Connect("192.168.1.11");
        try
        {
            prt2.Connect("192.168.1.12");            
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Lỗi, {ex.Message}");
        }



        // 7. Duyệt các thiết bị mạng thông qua kiểu INetworkable, không phụ thuộc vào lớp cụ thể
        Console.WriteLine(new string('-', 30));

            // Hủy kết nối mạng cũ của các thiết bị
        comp1.Disconnect();
        comp2.Disconnect();
        prt2.Disconnect();

        Console.WriteLine("7. Duyệt các thiết bị mạng thông qua kiểu INetworkable, không phụ thuộc vào lớp cụ thể");
        List<INetworkable> networkDevices = new List<INetworkable>
        {
            comp1, comp2, prt2
        };

        int ipCounter = 10;
        foreach (var netDevice in networkDevices)
        {
            netDevice.Connect($"192.168.1.{ipCounter++}");
        }
    }
}