
namespace QQ2564874169.Miniblink
{
	public class FormShadowWidth
	{
		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }

		public void SetAll(int width)
		{
			Left = Top = Right = Bottom = width;
		}

		internal FormShadowWidth()
		{
			
		}
	}
}
