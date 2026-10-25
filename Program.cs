/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Text;

namespace DeviceManagement;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        //1. KHOI TAO THIET BI VA KHO
        PrintHeader("1. KHỞI TẠO THIẾT BỊ VÀ KHO");

        var projectors = new InMemoryRepository<ProjectorRemote>();
        projectors.Add(new ProjectorRemote("PJ01", "Remote Epson 1", "Kho A", 15));
        projectors.Add(new ProjectorRemote("PJ02", "Remote Epson 2", "Kho A", 85));
        projectors.Add(new ProjectorRemote("PJ03", "Remote Epson 3", "Kho A", 65));

        var airRemotes = new InMemoryRepository<AirConditionerRemote>();
        airRemotes.Add(new AirConditionerRemote(
            "AC01", "Remote Daikin", "Kho B", new[] { "R301", "R302" }));
        airRemotes.Add(new AirConditionerRemote(
            "AC02", "Remote Panasonic", "Kho B", new[] { "A203" }));

        var microphones = new InMemoryRepository<WirelessMicrophone>();
        microphones.Add(new WirelessMicrophone("MIC01", "Micro 1", "Kho C", 40, 45));
        microphones.Add(new WirelessMicrophone("MIC02", "Micro 2", "Kho C", 85, 90));
        microphones.Add(new WirelessMicrophone("MIC03", "Micro 3", "Kho C", 20, 75));

        Console.WriteLine($"  Điều khiển máy chiếu: {projectors.GetAll().Count}");
        Console.WriteLine($"  Điều khiển điều hòa: {airRemotes.GetAll().Count}");
        Console.WriteLine($"  Micro không dây: {microphones.GetAll().Count}");

        Console.WriteLine($"  Tìm PJ02: {projectors.FindById("PJ02")}");
        Console.WriteLine($"  Xóa mã không tồn tại: {projectors.Remove("PJ99")}");
        Console.WriteLine($"  Máy chiếu sẵn sàng: {projectors.FindAll(d => d.IsReadyForUse()).Count}");
        Console.WriteLine($"  Micro sẵn sàng: {microphones.FindAll(d => d.IsReadyForUse()).Count}");

        //2. CAP PHAT BANG CHIEN LUOC
        PrintHeader("2. CẤP PHÁT THIẾT BỊ");

        var projectorService = new AllocationService<ProjectorRemote>(
            projectors,
            new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore()));

        ProjectorRemote selectedProjector = projectorService.Allocate();
        Console.WriteLine($"  Máy chiếu ưu tiên cao nhất: {selectedProjector.Id} " +
                          $"(pin {selectedProjector.BatteryLevel}) - {selectedProjector.Status}");

        var airService = new AllocationService<AirConditionerRemote>(
            airRemotes,
            new FirstAvailableStrategy<AirConditionerRemote>());

        AirConditionerRemote selectedAir =
            airService.Allocate(d => d.IsCompatibleWith("A203"));
        Console.WriteLine($"  Điều hòa cho A203: {selectedAir.Id} - {selectedAir.Status}");

        var microphoneService = new AllocationService<WirelessMicrophone>(
            microphones,
            new FirstAvailableStrategy<WirelessMicrophone>());

        WirelessMicrophone firstMicrophone = microphoneService.Allocate();
        Console.WriteLine($"  Micro đầu tiên sẵn sàng: {firstMicrophone.Id} - {firstMicrophone.Status}");

        //3. DOI CHIEN LUOC LUC CHAY
        PrintHeader("3. ĐỔI CHIẾN LƯỢC");

        microphoneService.ChangeStrategy(
            new HighestPriorityStrategy<WirelessMicrophone>(d => d.GetDefaultPriorityScore()));

        WirelessMicrophone bestMicrophone = microphoneService.Allocate();
        Console.WriteLine($"  Micro điểm cao nhất còn lại: {bestMicrophone.Id} " +
                          $"(điểm {bestMicrophone.GetDefaultPriorityScore():0.0})");

        //4. KIEM TRA NGOAI LE
        PrintHeader("4. KIỂM TRA NGOẠI LỆ");

        RunExceptionTest("Thêm mã thiết bị trùng", () =>
            projectors.Add(new ProjectorRemote("PJ01", "Remote trùng", "Kho A", 50)));

        RunExceptionTest("Không có micro phù hợp", () =>
            microphoneService.Allocate());

        RunExceptionTest("Mượn lại thiết bị đang Borrowed", () =>
            selectedProjector.Borrow());

        //5. TRA THIET BI VE KHO
        PrintHeader("5. TRẢ THIẾT BỊ");

        selectedProjector.Return();
        selectedAir.Return();
        firstMicrophone.Return();
        bestMicrophone.Return();

        Console.WriteLine($"  {selectedProjector.Id}: {selectedProjector.Status}");
        Console.WriteLine($"  {selectedAir.Id}: {selectedAir.Status}");
        Console.WriteLine($"  {firstMicrophone.Id}: {firstMicrophone.Status}");
        Console.WriteLine($"  {bestMicrophone.Id}: {bestMicrophone.Status}");

        //cung mot kho generic dung cho nhieu loai thiet bi
        Console.WriteLine($"  Repository dùng chung cho ProjectorRemote, " +
                          $"AirConditionerRemote và WirelessMicrophone.");
    }

    private static void RunExceptionTest(string title, Action action)
    {
        Console.WriteLine($"  {title}:");

        try
        {
            action();
            Console.WriteLine("    Không phát sinh ngoại lệ.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("    Kết thúc kiểm thử.");
        }
    }

    private static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {title} =====");
    }
}
