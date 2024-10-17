using System;
using System.Collections.Generic;

public interface IKemampuan
{
    void Gunakan(Robot robot, BosRobot bos);
    bool IsCooldown { get; }
    void UpdateCooldown();
}

public class ShieldBash : IKemampuan
{
    private int cooldown = 0;

    public bool IsCooldown => cooldown > 0;

    public void Gunakan(Robot robot, BosRobot bos)
    {
        if (!IsCooldown)
        {
            Console.WriteLine($"{robot.Nama} menggunakan Shield Bash!");
            bos.Stun(2); // Stun bos selama 2 turn
            cooldown = 3;
            Console.WriteLine($"{bos.Nama} tidak bisa bergerak selama 2 giliran!");
        }
        else
        {
            Console.WriteLine("Shield Bash sedang cooldown!");
        }
    }

    public void UpdateCooldown()
    {
        if (cooldown > 0) cooldown--;
    }
}

public class PlasmaCannon : IKemampuan
{
    private int cooldown = 0;

    public bool IsCooldown => cooldown > 0;

    public void Gunakan(Robot robot, BosRobot bos)
    {
        if (!IsCooldown)
        {
            Console.WriteLine($"{robot.Nama} menggunakan Plasma Cannon!");
            int damage = robot.Serangan * 2;
            bos.Energi -= damage;
            Console.WriteLine($"{bos.Nama} terkena {damage} damage oleh Plasma Cannon!");
            cooldown = 3;
        }
        else
        {
            Console.WriteLine("Plasma Cannon sedang cooldown!");
        }
    }

    public void UpdateCooldown()
    {
        if (cooldown > 0) cooldown--;
    }
}

public class ElectricShock : IKemampuan
{
    private int cooldown = 0;

    public bool IsCooldown => cooldown > 0;

    public void Gunakan(Robot robot, BosRobot bos)
    {
        if (!IsCooldown)
        {
            Console.WriteLine($"{robot.Nama} menggunakan Electric Shock!");
            int damage = robot.Serangan / 2;
            bos.Energi -= damage;
            Console.WriteLine($"{bos.Nama} terkena {damage} damage oleh Electric Shock!");
            cooldown = 2;
        }
        else
        {
            Console.WriteLine("Electric Shock sedang cooldown!");
        }
    }

    public void UpdateCooldown()
    {
        if (cooldown > 0) cooldown--;
    }
}

public class SuperShield : IKemampuan
{
    private int cooldown = 0;
    private bool shieldActive = false;

    public bool IsCooldown => cooldown > 0;

    public void Gunakan(Robot robot, BosRobot bos)
    {
        if (!IsCooldown)
        {
            Console.WriteLine($"{robot.Nama} mengaktifkan Super Shield!");
            shieldActive = true;
            cooldown = 3;
        }
        else
        {
            Console.WriteLine("Super Shield sedang cooldown!");
        }
    }

    public bool IsShieldActive => shieldActive;

    public void DisableShield()
    {
        shieldActive = false;
    }

    public void UpdateCooldown()
    {
        if (cooldown > 0) cooldown--;
    }
}

public class Repair : IKemampuan
{
    private int cooldown = 0;

    public bool IsCooldown => cooldown > 0;

    public void Gunakan(Robot robot, BosRobot bos)
    {
        if (!IsCooldown)
        {
            Console.WriteLine($"{robot.Nama} menggunakan Repair untuk memulihkan energi!");
            robot.Energi += 20;
            cooldown = 3;
        }
        else
        {
            Console.WriteLine("Repair sedang cooldown!");
        }
    }

    public void UpdateCooldown()
    {
        if (cooldown > 0) cooldown--;
    }
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Serangan { get; set; }
    public int Pertahanan { get; set; } // Properti pertahanan baru

    public Robot(string nama, int energi, int serangan, int pertahanan)
    {
        Nama = nama;
        Energi = energi;
        Serangan = serangan;
        Pertahanan = pertahanan; // Inisialisasi pertahanan
    }

    public void Serang(BosRobot bos)
    {
        int damage = Serangan - bos.Pertahanan; // Menghitung damage setelah pertahanan
        if (damage > 0)
        {
            bos.Energi -= damage;
            Console.WriteLine($"{Nama} menyerang {bos.Nama} dengan damage {damage}!");
        }
        else
        {
            Console.WriteLine($"{Nama} menyerang {bos.Nama}, tetapi serangan ditahan!");
        }
    }

    public abstract void GunakanKemampuan(BosRobot bos);
    public abstract void UpdateCooldown();

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Serangan: {Serangan}, Pertahanan: {Pertahanan}");
    }
}

public class Warrior : Robot
{
    private ShieldBash shieldBash = new ShieldBash();

    public Warrior(string nama, int energi, int serangan, int pertahanan) : base(nama, energi, serangan, pertahanan) { }

    public override void GunakanKemampuan(BosRobot bos)
    {
        Console.WriteLine("Pilih kemampuan: ");
        Console.WriteLine("1. Shield Bash");
        string pilihan = Console.ReadLine();
        if (pilihan == "1")
        {
            shieldBash.Gunakan(this, bos);
        }
        else
        {
            Console.WriteLine("Pilihan tidak valid! Robot tidak melakukan apa-apa.");
        }
    }

    public override void UpdateCooldown()
    {
        shieldBash.UpdateCooldown();
    }
}

public class Penembak : Robot
{
    private IKemampuan plasmaCannon = new PlasmaCannon();
    private IKemampuan electricShock = new ElectricShock();

    public Penembak(string nama, int energi, int serangan, int pertahanan) : base(nama, energi, serangan, pertahanan) { }

    public override void GunakanKemampuan(BosRobot bos)
    {
        Console.WriteLine("Pilih kemampuan: ");
        Console.WriteLine("1. Plasma Cannon");
        Console.WriteLine("2. Electric Shock");
        string pilihan = Console.ReadLine();
        switch (pilihan)
        {
            case "1":
                plasmaCannon.Gunakan(this, bos);
                break;
            case "2":
                electricShock.Gunakan(this, bos);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid! Robot tidak melakukan apa-apa.");
                break;
        }
    }

    public override void UpdateCooldown()
    {
        plasmaCannon.UpdateCooldown();
        electricShock.UpdateCooldown();
    }
}

public class Defender : Robot
{
    private SuperShield superShield = new SuperShield();
    private Repair repair = new Repair();

    public Defender(string nama, int energi, int serangan, int pertahanan) : base(nama, energi, serangan, pertahanan) { }

    public override void GunakanKemampuan(BosRobot bos)
    {
        Console.WriteLine("Pilih kemampuan: ");
        Console.WriteLine("1. Super Shield");
        Console.WriteLine("2. Repair");
        string pilihan = Console.ReadLine();
        switch (pilihan)
        {
            case "1":
                superShield.Gunakan(this, bos);
                break;
            case "2":
                repair.Gunakan(this, bos);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid! Robot tidak melakukan apa-apa.");
                break;
        }
    }

    public override void UpdateCooldown()
    {
        superShield.UpdateCooldown();
        repair.UpdateCooldown();
    }

    public bool IsShieldActive => superShield.IsShieldActive;
    public void DisableShield() => superShield.DisableShield();
}

public class BosRobot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Pertahanan { get; set; }
    public int Serangan { get; set; }
    private int stunTurns = 0;

    public BosRobot(string nama, int energi, int pertahanan, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Pertahanan = pertahanan;
        Serangan = serangan;
    }

    public void Diserang(Robot robot)
    {
        int damage = robot.Serangan - Pertahanan; 
        if (damage > 0)
        {
            Energi -= damage;
            Console.WriteLine($"{Nama} terkena {damage} damage!");
        }
        else
        {
            Console.WriteLine($"{Nama} berhasil menahan serangan!");
        }

        if (Energi <= 0)
        {
            Mati();
        }
    }

    public void Menyerang(Robot robot)
    {
        if (stunTurns > 0)
        {
            Console.WriteLine($"{Nama} tidak bisa bergerak karena ter-stun! ({stunTurns} giliran tersisa)");
            stunTurns--;
            return;
        }

        Random random = new Random();
        int damage = this.Serangan; // Normal damage
        // kemungkinan 20 persen damagenya di duakalipatkan
        if (random.Next(100) < 20) // membuat nomor acak dari 0 - 99
        {
            damage *= 2; //Setangan robot 2 kali lipat
            Console.WriteLine($"{Nama} menyerang dengan serangan ganda!");
        }

        robot.Energi -= damage;
        Console.WriteLine($"{robot.Nama} terkena {damage} damage dari {Nama}!");
    }

    public void Stun(int turns)
    {
        stunTurns = turns;
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} mati!");
    }

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama Bos: {Nama}, Energi: {Energi}, Serangan: {Serangan}, Pertahanan: {Pertahanan}");
    }
}



public class Game
{
    private Robot robot1;
    private Robot robot2;
    private BosRobot bosRobot;

    public Game(Robot robot1, Robot robot2, BosRobot bosRobot)
    {
        this.robot1 = robot1;
        this.robot2 = robot2;
        this.bosRobot = bosRobot;
    }

    public void MulaiPertarungan()
    {
        Console.WriteLine("Pertarungan dimulai!");

        while ((robot1.Energi > 0 || robot2.Energi > 0) && bosRobot.Energi > 0)
        {
            Console.WriteLine("\nGiliran Robot 1:");
            robot1.CetakInformasi();
            bosRobot.CetakInformasi();
            RobotAction(robot1);

            robot1.UpdateCooldown();

            if (bosRobot.Energi <= 0)
            {
                Console.WriteLine($"{bosRobot.Nama} kalah! Para robot menang!");
                break;
            }

            Console.WriteLine("\nGiliran Robot 2:");
            robot2.CetakInformasi();
            bosRobot.CetakInformasi();
            RobotAction(robot2);

            robot2.UpdateCooldown();

            if (bosRobot.Energi <= 0)
            {
                Console.WriteLine($"{bosRobot.Nama} kalah! Para robot menang!");
                break;
            }

            Console.WriteLine("\nGiliran Bos:");
            if (robot1.Energi > 0) bosRobot.Menyerang(robot1);
            if (robot2.Energi > 0) bosRobot.Menyerang(robot2);

            robot1.CetakInformasi();
            robot2.CetakInformasi();

            if (robot1.Energi <= 0 && robot2.Energi <= 0)
            {
                Console.WriteLine($"{bosRobot.Nama} menang!");
                break;
            }
        }
    }

    private void RobotAction(Robot robot)
    {
        Console.WriteLine("Pilih tindakan: ");
        Console.WriteLine("1. Serang Bos");
        Console.WriteLine("2. Gunakan Kemampuan");

        string pilihan = Console.ReadLine();

        switch (pilihan)
        {
            case "1":
                robot.Serang(bosRobot);
                break;
            case "2":
                robot.GunakanKemampuan(bosRobot);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid! Robot tidak melakukan apa-apa.");
                break;
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Pilih robot pertama:");
        Console.WriteLine("1. Penembak");
        Console.WriteLine("2. Defender");
        Console.WriteLine("3. Warrior");
        string pilihanRobot1 = Console.ReadLine();

        Robot robot1;
        switch (pilihanRobot1)
        {
            case "1":
                robot1 = new Penembak("Penembak-1", 100, 30,4);
                break;
            case "2":
                robot1 = new Defender("Defender-1", 120, 20,10);
                break;
            case "3":
                robot1 = new Warrior("Warrior-1", 110, 25,7);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid. Menggunakan Penembak secara default.");
                robot1 = new Penembak("Penembak-1", 100, 30,4);
                break;
        }

        Console.WriteLine("Pilih robot kedua:");
        Console.WriteLine("1. Penembak");
        Console.WriteLine("2. Defender");
        Console.WriteLine("3. Warrior");
        string pilihanRobot2 = Console.ReadLine();

        Robot robot2;
        switch (pilihanRobot2)
        {
            case "1":
                robot2 = new Penembak("Penembak-2", 100, 30,4);
                break;
            case "2":
                robot2 = new Defender("Defender-2", 120, 20,10);
                break;
            case "3":
                robot2 = new Warrior("Warrior-2", 110, 25,7);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid. Menggunakan Penembak secara default.");
                robot2 = new Penembak("Penembak-2", 100, 30,4);
                break;
        }

        BosRobot bos = new BosRobot("Bos Jahat", 150, 10,35);

        Game game = new Game(robot1, robot2, bos);
        game.MulaiPertarungan();
    }
}   