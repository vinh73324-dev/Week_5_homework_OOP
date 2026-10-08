/***********************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
************************/
using System;

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
    public Device(string id, string name, int year, int price, DeviceStatus status)
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

    #region Protected method
    protected int CalculateYearsUsed()
    {
        int yearsUsed = DateTime.Now.Year - PurchaseYear;
        return yearsUsed;
    }
    #region 

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
                    int price, DeviceStatus status, float ram,
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

