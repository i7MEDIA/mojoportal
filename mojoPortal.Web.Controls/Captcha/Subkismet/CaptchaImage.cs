using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Subkismet.Captcha;

public class CaptchaImage : IDisposable
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CaptchaImage"/> class.
	/// </summary>
	public CaptchaImage()
	{
		this.random = new Random();
		this.FontWarp = FontWarpFactor.Low;
		this.Width = 180;
		this.Height = 50;
	}

	#region Disposable Pattern
	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public virtual void Dispose()
	{
		GC.SuppressFinalize(this);
		this.Dispose(true);
	}

	/// <summary>
	/// Disposes the Captcha image.
	/// </summary>
	/// <param name="disposing">if set to <c>true</c> [disposing].</param>
	public virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.Image.Dispose();
		}
	}

	/// <summary>
	/// Releases unmanaged resources and performs other cleanup operations before the
	/// <see cref="CaptchaImage"/> is reclaimed by garbage collection.
	/// </summary>
	~CaptchaImage()
	{
		this.Dispose(false);
	}
	#endregion

	/// <summary>
	/// Generates a new Captcha image.
	/// </summary>
	public void GenerateImage()
	{
		this.image = this.GenerateImagePrivate();
	}

	private Bitmap GenerateImagePrivate()
	{
		Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			Font font;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			RectangleF rectF = new RectangleF(0f, 0f, this.width, this.height);
			Rectangle rect = new Rectangle(0, 0, this.width, this.height);
			HatchBrush smallConfettiBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
			graphics.FillRectangle(smallConfettiBrush, rect);
			float previousWidth = 0f;
			float size = Convert.ToInt32((this.height * 0.8));
			while (true)
			{
				font = new System.Drawing.Font(this.fontFamilyName, size, FontStyle.Bold);
				SizeF textSize = graphics.MeasureString(this.Text, font);
				if (textSize.Width <= this.width)
				{
					break;
				}
				if (previousWidth > 0f)
				{
					int estimatedSize = Convert.ToInt32(((textSize.Width - this.width) / (previousWidth - textSize.Width)));
					if (estimatedSize > 0)
					{
						size -= estimatedSize;
					}
					else
					{
						size -= 1f;
					}
				}
				else
				{
					size -= 1f;
				}
				previousWidth = textSize.Width;
			}
			size += 4f;

			font = new System.Drawing.Font(this.fontFamilyName, size, FontStyle.Bold);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			GraphicsPath textPath = new GraphicsPath();
			textPath.AddString(this.Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
			if (this.FontWarp != FontWarpFactor.None)
			{
				int warpDivisor = 0;
				switch (this._fontWarp)
				{
					case FontWarpFactor.Low:
						warpDivisor = 6;
						break;

					case FontWarpFactor.Medium:
						warpDivisor = 5;
						break;

					case FontWarpFactor.High:
						warpDivisor = 4;
						break;

					case FontWarpFactor.Extreme:
						warpDivisor = 3;
						break;
				}
				int heightRange = Convert.ToInt32((((double)rect.Height) / ((double)warpDivisor)));
				int widthRange = Convert.ToInt32((((double)rect.Width) / ((double)warpDivisor)));
				PointF point1 = this.RandomPoint(0, widthRange, 0, heightRange);
				PointF point2 = this.RandomPoint(rect.Width - (widthRange - Convert.ToInt32(point1.X)), rect.Width, 0, heightRange);
				PointF point3 = this.RandomPoint(0, widthRange, rect.Height - (heightRange - Convert.ToInt32(point1.Y)), rect.Height);
				PointF point4 = this.RandomPoint(rect.Width - (widthRange - Convert.ToInt32(point3.X)), rect.Width, rect.Height - (heightRange - Convert.ToInt32(point2.Y)), rect.Height);
				PointF[] points = new PointF[] { point1, point2, point3, point4 };
				Matrix matrix = new Matrix();
				matrix.Translate(0f, 0f);
				textPath.Warp(points, rectF, matrix, WarpMode.Perspective, 0f);
			}
			HatchBrush largeConfettiBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
			graphics.FillPath(largeConfettiBrush, textPath);

			int maxDimension = Math.Max(rect.Width, rect.Height);
			int steps = Convert.ToInt32((((double)(rect.Width * rect.Height)) / 30));
			for (int i = 0; i <= steps; i++)
			{
				graphics.FillEllipse(largeConfettiBrush, this.random.Next(rect.Width), this.random.Next(rect.Height), this.random.Next(Convert.ToInt32((((double)maxDimension) / 50))), this.random.Next(Convert.ToInt32((((double)maxDimension) / 50))));
			}
			font.Dispose();
			largeConfettiBrush.Dispose();
			graphics.Dispose();
		}
		return bitmap;
	}

	private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
	{
		return new PointF(this.random.Next(xmin, xmax), this.random.Next(ymin, ymax));
	}

	public string Font
	{
		get
		{
			return this.fontFamilyName;
		}
		set
		{
			try
			{
				using (System.Drawing.Font font1 = new System.Drawing.Font(value, 12f))
				{
					this.fontFamilyName = value;
					font1.Dispose();
				}
			}
			catch (Exception)
			{
				this.fontFamilyName = FontFamily.GenericSerif.Name;
			}
		}
	}

	/// <summary>
	/// Amount of random waping to apply to rendered text.
	/// </summary>
	/// <value>The font warp.</value>
	public FontWarpFactor FontWarp
	{
		get
		{
			return this._fontWarp;
		}
		set
		{
			this._fontWarp = value;
		}
	}

	/// <summary>
	/// Height of the Captcha image in pixels.
	/// </summary>
	public int Height
	{
		get
		{
			return this.height;
		}
		set
		{
			if (value <= 30)
			{
				throw new ArgumentOutOfRangeException("height", value, "height must be greater than 30.");
			}
			this.height = value;
		}
	}

	/// <summary>
	/// Gets the captcha image to display based on the current property 
	/// values.  Will render the image if it hasn't been rendered yet.
	/// </summary>
	/// <value>The image.</value>
	public Bitmap Image
	{
		get
		{
			if (this.image == null)
			{
				this.image = this.GenerateImagePrivate();
			}
			return this.image;
		}
	}

	/// <summary>
	/// Width of the Captcha image in pixels.
	/// </summary>
	public int Width
	{
		get
		{
			return this.width;
		}
		set
		{
			if (value <= 60)
			{
				throw new ArgumentOutOfRangeException("width", value, "width must be greater than 60.");
			}
			this.width = value;
		}
	}

	/// <summary>
	/// Gets or sets the text to render (warped of course).
	/// </summary>
	/// <value>The text.</value>
	public string Text
	{
		get { return this.text; }
		set { this.text = value; }
	}

	string text;

	private FontWarpFactor _fontWarp;
	private Bitmap image;
	private int height;
	private int width;
	private Random random;
	private string fontFamilyName;

	public enum FontWarpFactor
	{
		None,
		Low,
		Medium,
		High,
		Extreme
	}
}

