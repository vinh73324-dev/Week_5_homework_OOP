/***********************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
************************/
using System;
using System.Net;

public enum DeviceStatus
{
    Active,
    UnderMaintenance,
    Retired
}
public abstract class Device
{
    protected string DeviceId;
    protected string DeviceName;
    protected int PurchaseYear;
    protected decimal PurchasePrice;
    protected DeviceStatus Status;

    #region Constructor
    public Device(string id, string name, int year, decimal price, DeviceStatus status)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã thiết bị không được để trống");
        if (price <= 0)
            throw new ArgumentOutOfRangeException("Giá mua phải lớn hơn 0");
        if (year > DateTime.Now.Year)
            throw new ArgumentOutOfRangeException("Năm mua không được lớn hơn năm hiện tại");
        
        DeviceId = id;
        DeviceName = name;
        PurchasePrice = price;
        PurchaseYear = year;
        Status = status;
    }
    #endregion

    #region Abstract method
    public abstract decimal CalculateAnnualMaintenanceCost();
    #endregion

    #region Getter and Setter;
    public string GetDeviceID()
    {
        return DeviceId;   
    }

    public DeviceStatus GetStatus()
    {
        return Status;
    }
    #endregion

    #region Additional method
    //Tính số năm sử dụng
    public int CalculateYearsUsed()
    {
        int yearsUsed = DateTime.Now.Year - PurchaseYear;
        return yearsUsed;
    }
    #endregion

    #region Override method
    // Ghi đè ToString để trả về thông tin của thiết bị
    public override string ToString()
    {
        return $"Mã thiết bị: {DeviceId}\nTên thiết bị: {DeviceName}\nGiá mua: {PurchasePrice}\nNăm đưa vào sử dụng: {PurchaseYear}\nTrạng thái hoạt động: {Status}\n";
    }
    #endregion
}

public interface INetworkable
{
    string IpAddress { get; }
    void Connect(string ipAddress);
    void Disconnect();
    bool IsConnected { get; }
}

public class Computer: Device, INetworkable
{
    private float RamCapacity;
    private string CpuType;
    private bool HasDedicatedGpu;
    public string IpAddress { get; private set;}
    public bool IsConnected {get; private set;}
    
    #region  Constructor
    public Computer(string id, string name, int year, 
                    decimal price, DeviceStatus status, float ram,
                    string cpu, bool hasGpu)
        : base(id, name, year, price, status)
    {
        if (ram <= 0)
            throw new ArgumentOutOfRangeException("Dung lượng RAM phải lớn hơn 0");
        if (string.IsNullOrWhiteSpace(cpu))
            throw new ArgumentException("Tên CPU không được rỗng");
        
        RamCapacity = ram;
        CpuType = cpu;
        HasDedicatedGpu = hasGpu;
        IpAddress = string.Empty;
        IsConnected = false;
    }
    #endregion

    #region Override method
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = 0.05m * PurchasePrice;
        // Nếu có GPU rời
        if (HasDedicatedGpu == true)
            cost += 0.02m * PurchasePrice;
        // Nếu đã dùng trên 5 năm
        if (CalculateYearsUsed() > 5)
            cost += 0.01m * PurchasePrice;
        return cost;
    }

    public override string ToString()
    {
        string baseInfo = base.ToString();
        string moreInfo = $"Dung lượng RAM: {RamCapacity}\nLoại bộ xử lý: {CpuType}\n";
        if (HasDedicatedGpu == true)
            moreInfo += "Có GPU rời\n";
        else
            moreInfo += "Không có GPU rời\n";
        if (IsConnected == false)
            moreInfo += "Đang không kết nối mạng";
        else
            moreInfo += $"Đang kết nối mạng với IP: {IpAddress}";
        return baseInfo + moreInfo;
    }
    #endregion 

    #region Implement Interface
    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("Địa chỉ IP kết nối không được rỗng");
        if (IsConnected == true)
            throw new ArgumentException("Thiết bị đang kết nối với mạng khác, không thể kết nối thêm");
        IsConnected = true;
        IpAddress = ipAddress;
    }

    public void Disconnect()
    {
        IsConnected = false;
        IpAddress = string.Empty;
    }
    #endregion
}

public class Printer: Device
{
    protected string PrinterType;
    protected int PrintedPages;
    protected bool IsColorPrinter;

    #region Constructor
    public Printer(string id, string name, int year, decimal price,
                    DeviceStatus status, string type, int printedPages, bool isColorPrinter)
        : base(id, name, year, price, status)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Loại máy in không được để trống");
        if (printedPages < 0)
            throw new ArgumentOutOfRangeException("Số trang đã in phải là số không âm");
        PrinterType = type;
        PrintedPages = printedPages;
        IsColorPrinter = isColorPrinter;
    }
    #endregion

    #region Override method
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = 0.04m * PurchasePrice;
        if (PrintedPages > 100000)
            cost += 500000m;
        if (IsColorPrinter == true)
            cost += 300000m;
        return cost;
    }

    public override string ToString()
    {
        string baseInfo = base.ToString();
        string moreInfo = $"Loại máy in: {PrinterType}\nSố trang đã in: {PrintedPages}\n";
        if (IsColorPrinter == true)
            moreInfo += "Là máy in màu\n";
        else
            moreInfo += "Là máy in không màu\n";
        return baseInfo + moreInfo;
    }
    #endregion
}

public class NetworkPrinter: Printer, INetworkable
{
    public string IpAddress { get; private set; }
    public bool IsConnected{ get; private set; }

    #region Constructor
    public NetworkPrinter(string id, string name, int year, decimal price, 
                        DeviceStatus status, string type, int printedPages, bool isColorPrinter)
        : base(id, name, year, price, status, type, printedPages, isColorPrinter)
    {
        IpAddress = string.Empty;
        IsConnected = false;
    }
    #endregion

    #region Implement Interface
    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("Địa chỉ IP kết nối không được rỗng");
        if (IsConnected == true)
            throw new ArgumentException("Thiết bị đang kết nối với mạng khác, không thể kết nối thêm");
        IsConnected = true;
        IpAddress = ipAddress;
    }

    public void Disconnect()
    {
        IsConnected = false;
        IpAddress = string.Empty;
    }
    #endregion  

    #region Override method
    public override string ToString()
    {
        string baseInfo = base.ToString();
        if (IsConnected == false)
            return baseInfo;
        else
            return baseInfo + $"Địa chỉ IP: {IpAddress}";
    }
    #endregion

}

public class Projector: Device
{
    private int BrightnessLumens;
    private int HoursUsed;

    #region Constructor
    public Projector(string id, string name, int year, decimal price,
                    DeviceStatus status, int brightness, int hoursUsed)
        : base(id, name, year, price, status)
    {
        if (brightness <= 0)
            throw new ArgumentOutOfRangeException("Độ sáng phải dương");
        if (hoursUsed < 0)
            throw new ArgumentOutOfRangeException("Số giờ sử dụng không được âm");

        BrightnessLumens = brightness;
        HoursUsed = hoursUsed;
    }

    #endregion

    #region Override method
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = 0.03m * PurchasePrice;
        if (HoursUsed > 3000)
            cost += 1500000;
        return cost;
    }

    public override string ToString()
    {
        string baseInfo = base.ToString();
        string moreInfo = $"Độ sáng: {BrightnessLumens} Lumen\nSố giờ sử dụng: {HoursUsed}";
        return baseInfo + moreInfo;
    }
    #endregion
}

public class LabRoom
{
    private string RoomId;
    private string RoomName;
    private int Capacity;
    private List<Device> Devices;

    #region Constructor
    public LabRoom(string id, string name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã phòng thí nghiệm không được để trống");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên phòng thí nghiệm không được để trống");
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException("Sức chứa phải dương");
        RoomId = id;
        RoomName = name;
        Capacity = capacity;
        Devices = new List<Device>();
    }
    #endregion

    #region Public method
    public void AddDevice(Device device)
    {
        if (device == null)
            throw new ArgumentNullException("Không thể thêm thiêt bị NULL");
        // Kiểm tra sự trùng lặp mã thiết bị
        bool isDuplicate = Devices.Any(d => d.GetDeviceID().Equals(device.GetDeviceID(), StringComparison.OrdinalIgnoreCase));
        if (isDuplicate)
            throw new InvalidOperationException("Không thể thêm thiết bị đã tồn tại vào phòng");
        if (Devices.Count >= Capacity)
            throw new InvalidOperationException("Sức chứa của phòng đã đầy, không thể thêm");
        Devices.Add(device);
    }

    public Device? FindDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            return null;
        return Devices.Find(d => d.GetDeviceID().Equals(deviceId, StringComparison.OrdinalIgnoreCase));
    }

    public bool RemoveDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            return false;
        Device? DeviceToRemove = FindDevice(deviceId);
        if (DeviceToRemove == null)
            return false;
        return Devices.Remove(DeviceToRemove);
    }

    public List<Device> GetDevicesRequiringMaintenance()
    {
        List<Device> DeviceNeedMaintenance = new List<Device>();
        foreach (Device d in Devices)
        {
            if (d.GetStatus() == DeviceStatus.UnderMaintenance || DateTime.Now.Year - d.CalculateYearsUsed() > 5)
                DeviceNeedMaintenance.Add(d);
        }
        return DeviceNeedMaintenance;
    }
    #endregion

    #region Polymorphism method
    public decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = 0.0m;
        foreach(Device d in Devices)
            cost += d.CalculateAnnualMaintenanceCost();
        return cost;
    }
    #endregion
}