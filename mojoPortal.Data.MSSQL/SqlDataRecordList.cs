/*
 * 
 * Adapted from Erland Sommarskog's article at
 * https://www.sommarskog.se/arrays-in-sql-2008.html
 * Note: While Erland wrote that article in 2010, his last update to it was in 2020, so he's updating it as needed.
 * 
 */

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;

namespace mojoPortal.Data 
{
	/// <summary>
	/// Creates a list of SqlDataRecord from a list of values. This new list is used to populate a TVP (Table-Valued Parameter) in a sproc.
	/// </summary>
	public class SqlDataRecordList : IEnumerable<SqlDataRecord>, IEnumerator<SqlDataRecord>
	{
		string input;         // The input string.
		char delim;         // The delimiter.
		System.Data.SqlDbType type; // data type
		int start_ix;      // Start position for current list element.
		int end_ix;        // Position for the next list delimiter.
		SqlDataRecord outrec;        // The record we use to return data.

		/// <summary>
		/// Create a SqlDataRecordList from a List<int>
		/// </summary>
		/// <param name="list"></param>
		public SqlDataRecordList(List<int> list) : this(string.Join(",", list), ',', System.Data.SqlDbType.Int) { }

		/// <summary>
		/// Create a SqlDataRecordList from a List<Guid>
		/// </summary>
		/// <param name="list"></param>
		public SqlDataRecordList(List<Guid> list) : this(string.Join(",", list), ',', System.Data.SqlDbType.UniqueIdentifier) { }

		/// <summary>
		/// Create a SqlDataRecordList from a string
		/// </summary>
		/// <param name="str"></param>
		public SqlDataRecordList(string str) : this(str, ',', System.Data.SqlDbType.BigInt) { }

		/// <summary>
		/// Create a SqlDataRecordList from a string, specifying the delimiter and SqlDbType
		/// </summary>
		/// <param name="str"></param>
		/// <param name="delimiter"></param>
		/// <param name="sqlDbType"></param>
		public SqlDataRecordList(string str, char delimiter, System.Data.SqlDbType sqlDbType)
		{
			// Save input string and delimiter.
			input = str;
			delim = delimiter;
			type = sqlDbType;

			// Create an SqlDataRecord to return in the Current method.
			outrec = new SqlDataRecord(new SqlMetaData("nnnn", this.type));

			// Perform the Reset() operation for the rest of the initiation.
			Reset();
		}

		// GetEnumerator - part of IEnumerable. There are two of them since this
		// is required by the generic class. Since we also implement IEnumerator
		// we just return ourselves.
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this;
		}

		public System.Collections.Generic.IEnumerator<SqlDataRecord> GetEnumerator()
		{
			return this;
		}

		// Reset - part of IEnumerable. We set current position in the string
		// to be before the string.
		public void Reset()
		{
			start_ix = -1;
			end_ix = -1;
		}

		// MoveNext - part of IEnumerable. We move start_ix and end_ix to
		// the next element in the list.
		public bool MoveNext()
		{
			start_ix = this.end_ix + 1;

			// There may be multiple adjacent delimiters, that is, empty
			// list elements. We skip until we find a character that is
			// not a delimitere.
			while (start_ix < input.Length && input[start_ix] == delim)
			{
				start_ix++;
			}

			// If we did not find any non-delimiter, we have exhausted the
			// string, and we should return false to tell caller that we're done.
			if (start_ix >= input.Length)
			{
				return false;
			}

			// Find the next delimiter. If there are no more delimiters, we
			// say that there is one after the end of the list.
			end_ix = input.IndexOf(delim, start_ix);
			if (end_ix == -1)
			{
				end_ix = input.Length;
			}

			// Return true since there is at least one more elment
			return true;
		}

		// Current - part if IEnumerable. Extract the current list value and
		// return it in the SqlDataRecord.
		public SqlDataRecord Current
		{
			get
			{
				string str = input.Substring(start_ix, end_ix - start_ix);
				switch (type)
				{
					case System.Data.SqlDbType.Int:
						//outrec.SetInt32(0, Convert.ToInt32(str));
						outrec.SetSqlInt32(0, Convert.ToInt32(str));
						break;
					case System.Data.SqlDbType.BigInt:
						outrec.SetInt64(0, Convert.ToInt64(str));
						break;
					case System.Data.SqlDbType.UniqueIdentifier:
						outrec.SetSqlGuid(0, Guid.Parse(str));
						break;
				}
				
				return outrec;
			}
		}

		// IEnumerable<T> requires that we also implement a non-generic Current.
		Object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Dispose is required for IEnumerable.
		public void Dispose()
		{
			outrec = null;
		}

		// We override ToString() to make debugging more interesting.
		public override string ToString()
		{
			return "SqlDataRecordList: " + input;
		}
	}
}