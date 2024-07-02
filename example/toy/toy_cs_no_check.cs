    [DllImport("libc", EntryPoint = "sync", SetLastError = false)]
    public static extern void sync();

    public static void example_toy() {


        using (var file = File.Create("tmp"))
            file.Write("world"u8);
        
        sync();


        File.Move("tmp", "file1", true);
        sync();

        Console.WriteLine("Updated");
    }