    [DllImport("libc", EntryPoint = "sync", SetLastError = false)]
    public static extern void sync();

    public static void example_toy() {


        using (FileStream fs = new FileStream("tmp", FileMode.OpenOrCreate, FileAccess.Write))
        {
            byte[] info = new System.Text.UTF8Encoding(true).GetBytes("world");
            fs.Write(info, 0, info.Length);
        }
        
        sync();


        File.Move("tmp", "file1", true);
        sync();

        Console.WriteLine("Updated");
    }