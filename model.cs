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

    #region Override method
    // Ghi đè ToString để trả về thông tin của thiết bị
    public override string ToString()
    {
        return $"Mã thiết bị: {DeviceId}\nTên thiết bị: {DeviceName}\nGiá mua: {PurchasePrice}\nNăm đưa vào sử dụng: {PurchaseYear}\nTrạng thái hoạt động: {Status}";
    }
    #endregion
}
