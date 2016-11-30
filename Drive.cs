using System.Windows.Forms;
using System.Drawing;
using System;

public class Driver
{
    private Form screen;
    private Panel canvas;
    private Image auto;
    private Image other;
    private Image bg;
    private Image bloc;
    private Graphics g;
    private int x = 1280;
    private int y = 60;
    private int width = 100;
    private int height = 60;
    private Random rnd = new Random();
    private System.Collections.Generic.List<Row> rows;
    private System.Collections.Generic.List<Other> others;

    private bool startis = true;

    private int tx = 300;
    private int ty = 500;
    private int thex = -1;

    private int life = 12;

    public Driver()
    {
        auto = Image.FromFile(Environment.CurrentDirectory + "\\auto.png");
        bg = Image.FromFile(Environment.CurrentDirectory + "\\bg.png"); 
        screen = new Form();
        canvas = new Panel();
        screen.SetBounds(0,0,1280,1024);
        canvas.SetBounds(0,0,1280,1024);
        g = canvas.CreateGraphics();
        screen.Controls.Add(canvas);
        screen.WindowState = FormWindowState.Maximized;
        bloc = Image.FromFile(Environment.CurrentDirectory + "\\block.png");
        other = Image.FromFile(Environment.CurrentDirectory + "\\other.png");
    }

    public void Play()
    {
        Timer loop = new Timer();
        loop.Interval += 1200;
        loop.Tick += new EventHandler(Drive);
        screen.KeyDown += new KeyEventHandler(Move);
        rows = new System.Collections.Generic.List<Row>();
        others = new System.Collections.Generic.List<Other>();
        int count = 0;
        while (true)
        {
            Drive(null, null);
            count++;
            if (count > 100)
                break;
        }
        thex = x;
        startis = false;
        loop.Start();
    }

    private void Move(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Up)
        {
            ty -= 20;
        }
        
        if (e.KeyCode == Keys.Down)
        {
            ty += 20;
        }
    }

    int count = 0;
    private void Drive(object sender, EventArgs e)
    {
        screen.Text = "Energy: " + life;

        if (startis)
        {
            tx = 520;
            ty = y + 100;
        }
        try
        {
            g.DrawImage(auto, tx, ty, 50, 50);
        }
        catch (Exception ex)
        {
            int vx;
        }

        try
        {
            g.DrawImage(bg, x, 0, canvas.Width, canvas.Height);
        }
        catch (Exception ex)
        {
            int vx;
        }

        try
        {
            g.DrawImage(bg, x - 1280, 0, canvas.Width, canvas.Height);
        }
        catch (Exception ex)
        {
            int vx;
        }

        if (x < 0)
        {
            x = 1300;
        }
        x -= 60;

        int v = 0;
        int w = 0;
        int xx = rnd.Next(5000);
        if (xx < 2500)
        {
            v = rnd.Next(2) * (rnd.Next(20) + 30);
        }
        else if (xx > 2500)
        {
            w = rnd.Next(2) * (rnd.Next(20) + 30);
        }
        y += v;
        y -= w;

        if (y < 100)
        {
            y += 60;
        }

        if (y > 700)
        {
            y -= 60;
        }

        Row row = new Row();
        row.SetLocation(1280, y);
        int xy = rnd.Next(10);
        if (xy == 0)
        {
            Other car = new Other();
            car.x = 1280;
            car.y = y;
            others.Add(car);
        }
        int px = rnd.Next(10);
        if (px == 0)
            row.AddBlockAbove();
        if (px == 1)
            row.DelBlockAbove();
        if (px == 2)
            row.AddBlockBelow();
        if (px == 3)
            row.DelBlockBelow();
        if (rows.Count != 0)
        {
            rows.Insert(rows.Count - 1, row);
        }
        else
        {
            rows.Add(row);
        }
        for (int i = 0; i < rows.Count; i++)
        {
            System.Collections.Generic.List<Block> blocks = rows[i].blocks;
            for (int j = 0; j < blocks.Count; j++)
            {
                int k = j + 1;
                int l = blocks.Count / 2;
                Block block = blocks[j];
                block.x -= 100;
                try
                {
                    g.FillRectangle(new SolidBrush(Color.Gray), block.x, block.y, width, height);
                    if (k == l + 1)
                        g.FillRectangle(new SolidBrush(Color.White), block.x, block.y, 40, 6);
                    //g.DrawImage(bloc, block.x, block.y, width, height);
                }
                catch (Exception ex)
                {
                    int vx;
                }
            }
            for (int k = 0; k < others.Count; k++)
            {
                Other car = others[k];
                car.x -= 100;
                g.DrawImage(other, car.x, car.y, 30, 30);
            }
        }

        if(!startis)
        {
            Block block = rows[-14+count].blocks[0];
            int they = block.y;
            if (ty < they)
            {
                life--;
            }
            block = rows[-14+count].blocks[rows[-14+count].blocks.Count - 1];
            they = block.y;
            if (ty > they)
            {
                life--;
            }
        }

        if (life <= 0)
        {
            Application.Exit();
        }

        count++;
    }

    public class Row
    {
        public System.Collections.Generic.List<Block> blocks;

        public Row()
        {
            blocks = new System.Collections.Generic.List<Block>();

            Block block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
            block = new Block();
            blocks.Add(block);
        }

        public void SetLocation(int x, int y)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].x = x;
                blocks[i].y = y + (i * 60);
            }
        }

        public void AddBlockAbove()
        {
            Block block = new Block();
            block.x = blocks[0].x;
            block.y = blocks[0].y - 60;
            blocks.Insert(0, block);
        }

        public void AddBlockBelow()
        {
            if (blocks.Count >= 1)
            {
                Block block = new Block();
                block.x = blocks[blocks.Count - 1].x;
                block.y = blocks[blocks.Count - 1].y + 60;
                blocks.Add(block);
            }
        }

        public void DelBlockAbove()
        {
            blocks.RemoveAt(0);
        }

        public void DelBlockBelow()
        {
            blocks.RemoveAt(blocks.Count - 1);
        }
    }

    public class Block
    {
        public int x;
        
        public int y;
    }

    public class Other
    {
        public int x;

        public int y;
    }

    public static void Main(string[] args)
    {
        Driver d = new Driver();
        d.Play();
        Application.Run(d.screen);
    }
}