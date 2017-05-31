namespace RSBuild
{
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
	/// Represents a report.
	/// </summary>
	[Serializable]
	public class BinaryFile
	{
		private string _Name;
		private string _FilePath;
		private string _CollapsedHeight;
		private CacheOption _CacheOption;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get
			{
				return _Name;
			}
		}

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
		public string FilePath
		{
			get
			{
				return _FilePath;
			}
		}

        /// <summary>
        /// Gets the collapsed height of the report.
        /// </summary>
        /// <value>The collapsed height of the report.</value>
		public string CollapsedHeight
		{
			get
			{
				return _CollapsedHeight;
			}
		}

        /// <summary>
        /// Gets the cache option.
        /// </summary>
        /// <value>The cache option.</value>
		public CacheOption CacheOption
		{
			get
			{
				return _CacheOption;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="collapsedHeight">The height.</param>
        /// <param name="cacheTime">The cache time.</param>
		public BinaryFile(string name, string filePath, string collapsedHeight, int cacheTime)
		{
			_Name = name;
			_FilePath = filePath;
			if (collapsedHeight != null && Util.ValidateDistance(collapsedHeight))
			{
				_CollapsedHeight = collapsedHeight;
			}

			if (cacheTime > 0)
			{
				_CacheOption = new CacheOption(cacheTime);
			}
		}

        /// <summary>
        /// Processes the report.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns>The report definition.</returns>
		public byte[] Process(string targetFolder)
		{
            
			    System.IO.FileStream st  = null;
                byte[] data = null;
            try
			{
            st = new System.IO.FileStream(_FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            data = new byte[st.Length];
            st.Read(data, 0, (int)st.Length);
           
			
               
             
			}
			catch(Exception e)
			{
				Logger.LogException("BinaryFile::Process", e.Message);
			}
			finally
			{
                if (st != null)
                {
                    st.Close();
                }
			}
            return data;
		}

        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Collapses the height.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="xnm">The namespace.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
		private XmlDocument CollapseHeight(XmlDocument input, XmlNamespaceManager xnm, string height)
		{
			XmlDocument output = input;

			try
			{
				XmlNode n1 = output.SelectSingleNode("//def:BinaryFile/def:Body/def:Height", xnm);
				if (n1 != null)
				{
					n1.InnerText = height;
				}

			}
			catch(Exception)
			{}

			return output;
		}

        /// <summary>
        /// Configs the data source.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="xnm">The namespace.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="targetFolder">The target folder.</param>
        /// <returns></returns>
		private XmlDocument ConfigDataSource(XmlDocument input, XmlNamespaceManager xnm, DataSource dataSource, string targetFolder)
		{
			XmlDocument output = input;

				if (dataSource.Publish)
				{
					string nodeXml = string.Format("<rd:DataSourceID>{0}</rd:DataSourceID><DataSourceReference>{1}</DataSourceReference>",
						Guid.NewGuid().ToString(),
						string.Format("{0}{1}", Util.GetRelativePath(targetFolder, dataSource.TargetFolder), dataSource.Name)
						);

					try
					{
						XmlNode n1 = output.SelectSingleNode("//def:BinaryFile/def:DataSources/def:DataSource", xnm);
						if (n1 != null)
						{
							n1.InnerXml = nodeXml;
						}

					}
					catch(Exception)
					{}
				}

			return output;
		}
	}
}
