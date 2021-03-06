﻿using System;
using System.Runtime.Serialization;

namespace SSG.PDF.Exceptions
{

    /// <summary>
    /// Exception class of receiving message queue error event.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    [Serializable]
    public class ReadMessageQueueException : Exception, ISerializable
    {

        #region Constructor

            /// <summary>
            /// Contructs a ReadMessageQueueException with no detail message.
            /// </summary>
            public ReadMessageQueueException()
            {
            }

            /// <summary>
            /// Constructs a ReadMessageQueueException with the specified detail message.
            /// A detail message is a String that describes this particular exception.
            /// </summary>
            /// <param name="strMessage">the detail message.</param>
            public ReadMessageQueueException(string strMessage)
                : base(strMessage)
            {
            }

            /// <summary>
            /// Constructs a ReadMessageQueueException with the specified detail message and push the exception object detail.
            /// A detail message is a String that describes this particular exception.
            /// An exception object that describes details this particular exception.
            /// </summary>
            /// <param name="strMessage">The error message that explains the reason for the exception.</param>
            /// <param name="oInner">The exception that is the cause of 
            /// the current exception, or a null reference if no inner exception is specified.</param>
            public ReadMessageQueueException(string strMessage, Exception oInner)
                : base(strMessage, oInner)
            {
            }

            /// <summary>
            /// Constructs a ReadMessageQueueException with the specified serialize information and stream context.
            /// </summary>
            /// <param name="oInfo">The System.Runtime.Serialization.SerializationInfo 
            /// that holds the serialized object data about the exception being thrown.</param>
            /// <param name="oContext">The System.Runtime.Serialization.StreamingContext 
            /// that contains contextual information about the source or destination.</param>
            protected ReadMessageQueueException(SerializationInfo oInfo, StreamingContext oContext)
                : base(oInfo, oContext)
            {
            }

        #endregion

    }
}
