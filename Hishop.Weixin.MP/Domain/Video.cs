using System;
namespace Hishop.Weixin.MP.Domain
{
	public class Video : IMedia, IThumbMedia
	{
		public string MediaId
		{
			get;
			set;
		}
		public int ThumbMediaId
		{
			get;
			set;
		}
	}
}
