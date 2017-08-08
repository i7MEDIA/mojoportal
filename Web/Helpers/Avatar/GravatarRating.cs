namespace mojoPortal.Web.Helpers
{
	/// <summary>
	/// Enum specifying the possible age-ratings to limit our gravatar images to.
	/// </summary>
	public enum GravatarRating
	{
		/// <summary>
		/// Suitable for display on all websites with any audience type.
		/// </summary>
		G,

		/// <summary>
		/// May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence.
		/// </summary>
		PG,

		/// <summary>
		/// May contain such things as harsh profanity, intense violence, nudity, or hard drug use.
		/// </summary>
		R,

		/// <summary>
		/// May contain hardcore sexual imagery or extremely disturbing violence.
		/// </summary>
		X
	}
}