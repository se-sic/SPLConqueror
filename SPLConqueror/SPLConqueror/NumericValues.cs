using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
	/// <summary>
    /// This class represents multiple numeric values.
    /// </summary>
	public class NumericValues : IEnumerable<double>
    {
		private double [] values;

        /// <summary>
        /// Returns the values.
        /// </summary>
        /// <value>The values of this class.</value>
		public double [] Values {
			get {
				return values;
			}
			set { 
				List<double> temp = value.ToList();
				temp.Sort ((double x, double y) => x.CompareTo (y));
				this.values = temp.ToArray();
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SPLConqueror_Core.NumericValues"/> class.
		/// Before assignment, the values in the array are sorted.
        /// </summary>
        /// <param name="values">The values for initializing the class.</param>
        public NumericValues (double[] values)
        {
			this.Values = values;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SPLConqueror_Core.NumericValues"/> class.
        /// </summary>
		/// <param name="values">The values for initializing the class as string (e.g., '(1;2;3)').</param>
		public NumericValues (String values) : this (values.Replace ("(", "").Replace (")", "").Split (';').Select (Double.Parse).ToArray ())
		{
		}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:SPLConqueror_Core.NumericValues"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:SPLConqueror_Core.NumericValues"/>.</returns>
		public override String ToString ()
		{
			StringBuilder stringBuilder = new StringBuilder ();

			for (int i = 0; i < values.Length; i++) {
				stringBuilder.Append (values[i]);
				if (i < values.Length - 1) {
					stringBuilder.Append (";");
				}
			}

			return stringBuilder.ToString ();
		}

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
		public IEnumerator GetEnumerator ()
		{
			return values.GetEnumerator ();
		}

		/// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
		IEnumerator<double> IEnumerable<double>.GetEnumerator ()
		{
			return ((IEnumerable<double>)values).GetEnumerator ();
		}
	}
}
