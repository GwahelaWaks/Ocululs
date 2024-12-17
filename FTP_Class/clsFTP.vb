Option Explicit On
Option Strict On

Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Sockets


Namespace Oculus10.FTPandMail.FTP
    'FTP Class
    Public Class clsFTP

#Region "Class Variable Declarations"
        Private m_sRemoteHost, m_sRemotePath, m_sRemoteUser As String
        Private m_sRemotePassword, m_sMess As String
        Private m_iRemotePort, m_iBytes As Int32
        Private m_objClientSocket As Socket
        Private m_iClientSocketTimeout As Integer = 180000 '180 seconds

        'Public m_oEventLogging As New Oculus_FTP_Dll.EventLogging

        Private m_iRetValue As Int32
        Private m_bLoggedIn As Boolean
        Private m_sMes, m_sReply As String

        'Private m_oOculet As New Oculus.Oculets.QueueToDirectory.Oculet

        'Set the size of the packet that is used to read and to write data to the FTP server 
        'to the following specified size.
        Public Const BLOCK_SIZE As Integer = 512
        Private m_aBuffer(BLOCK_SIZE) As Byte
        Private ASCII As Encoding = Encoding.ASCII
        Public flag_bool As Boolean
        'General variable declaration
        Private m_sMessageString As String
#End Region

#Region "Class Constructors"

        ' Main class constructor
        Public Sub New()
            m_sRemoteHost = "microsoft"
            m_sRemotePath = "."
            m_sRemoteUser = "anonymous"
            m_sRemotePassword = ""
            m_sMessageString = ""
            m_iRemotePort = 21
            m_bLoggedIn = False
        End Sub

        ' Parameterized constructor
        Public Sub New(ByVal sRemoteHost As String, _
                       ByVal sRemotePath As String, _
                       ByVal sRemoteUser As String, _
                       ByVal sRemotePassword As String, _
                       ByVal iRemotePort As Int32)
            m_sRemoteHost = sRemoteHost
            m_sRemotePath = sRemotePath
            m_sRemoteUser = sRemoteUser
            m_sRemotePassword = sRemotePassword
            m_sMessageString = ""
            m_iRemotePort = iRemotePort
            m_bLoggedIn = False
        End Sub
#End Region

#Region "Public Properties"

        'Set or Get the name of the FTP server that you want to connect to.
        Public Property RemoteHostFTPServer() As String
            'Get the name of the FTP server. 
            Get
                Return m_sRemoteHost
            End Get
            'Set the name of the FTP server. 
            Set(ByVal Value As String)
                m_sRemoteHost = Value
            End Set
        End Property

        'Set or Get the FTP port number of the FTP server that you want to connect to.
        Public Property RemotePort() As Int32
            'Get the FTP port number. 
            Get
                Return m_iRemotePort
            End Get
            'Set the FTP port number. 
            Set(ByVal Value As Int32)
                m_iRemotePort = Value

            End Set
        End Property

        'Set or Get the remote path of the FTP server that you want to connect to.
        Public Property RemotePath() As String
            'Get the remote path. 
            Get
                Return m_sRemotePath
            End Get
            'Set the remote path. 
            Set(ByVal Value As String)
                m_sRemotePath = Value
            End Set
        End Property

        'Set the remote password of the FTP server that you want to connect to.
        Public Property RemotePassword() As String
            Get
                Return m_sRemotePassword
            End Get
            Set(ByVal Value As String)
                m_sRemotePassword = Value
            End Set
        End Property

        'Set or Get the remote user of the FTP server that you want to connect to.
        Public Property RemoteUser() As String
            Get
                Return m_sRemoteUser
            End Get
            Set(ByVal Value As String)
                m_sRemoteUser = Value
            End Set
        End Property

        'Set the class messagestring.
        Public Property MessageString() As String
            Get
                Return m_sMessageString
            End Get
            Set(ByVal Value As String)
                m_sMessageString = Value
            End Set
        End Property

#End Region

#Region "Public Subs and Functions"

        'Return a list of files from the file system. Return these files in a string() array.
        Public Function GetFileList(ByVal sMask As String) As String()
            Dim cSocket As Socket
            Dim bytes As Int32
            Dim seperator As Char = ControlChars.Lf
            Dim mess() As String

            m_sMes = ""
            'Check to see if you are logged on to the FTP server.
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("GetFileList - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If

            cSocket = CreateDataSocket()
            'Send an FTP command. 
            SendCommand("NLST " & sMask)

            If (Not (m_iRetValue = 150 Or m_iRetValue = 125)) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            m_sMes = ""
            Do While (True)
                m_aBuffer.Clear(m_aBuffer, 0, m_aBuffer.Length)
                bytes = cSocket.Receive(m_aBuffer, m_aBuffer.Length, 0)
                m_sMes += ASCII.GetString(m_aBuffer, 0, bytes)

                If (bytes < m_aBuffer.Length) Then
                    Exit Do
                End If
            Loop

            mess = m_sMes.Split(seperator)
            cSocket.Close()
            ReadReply()


            If (m_iRetValue <> 226) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            Return mess
        End Function

        'Return a list of files & Directories from the file system.
        Public Sub GetDirList(ByVal dl As DirList)
            Dim cSocket As Socket
            Dim bytes As Int32
            Dim seperator As Char = ControlChars.Lf
            Dim mess() As String

            m_sMes = ""
            'Check to see if you are logged on to the FTP server.
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("GetDirList - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If

            cSocket = CreateDataSocket()
            'Send an FTP command. 
            SendCommand("LIST -AL")

            If (Not (m_iRetValue = 150 Or m_iRetValue = 125)) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            m_sMes = ""
            Do While (True)
                m_aBuffer.Clear(m_aBuffer, 0, m_aBuffer.Length)
                bytes = cSocket.Receive(m_aBuffer, m_aBuffer.Length, 0)
                m_sMes += ASCII.GetString(m_aBuffer, 0, bytes)

                If (bytes < m_aBuffer.Length) Then
                    Exit Do
                End If
            Loop

            mess = m_sMes.Split(seperator)
            cSocket.Close()
            ReadReply()

            If (m_iRetValue <> 226) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            Dim cSep() As Char = {CChar(vbCr), CChar(vbLf)}
            Dim tmpstr, c As String
            Dim strItems() As String

            If mess.Length > 0 Then
                For Each tmpstr In mess
                    If tmpstr <> "" Then
                        Dim lf As DirFile
                        Dim n, d, t, s As String    'name, date, time & either size or <dir>
                        strItems = tmpstr.Split(" "c)

                        Dim x As Integer = 0
                        For Each c In strItems
                            If c.Length > 0 Then
                                x += 1
                                Select Case x
                                    Case 1
                                        d = c.Replace(vbCr, "")
                                    Case 2
                                        t = c.Replace(vbCr, "")
                                    Case 3
                                        s = c.Replace(vbCr, "")
                                    Case 4
                                        n = c.Replace(vbCr, "")
                                End Select
                            End If
                        Next

                        If s = "<DIR>" Then
                            lf = New DirFile(n, d, t, True)
                        Else
                            lf = New DirFile(n, d, t, , CInt(s))
                        End If
                        dl.Add(lf)
                    End If
                Next
            End If

        End Sub

        ' Get the size of the file on the FTP server.
        Public Function GetFileSize(ByVal sFileName As String) As Long
            Dim size As Long

            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("GetFileSize - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command. 
            SendCommand("SIZE " & sFileName)
            size = 0

            If (m_iRetValue = 213) Then
                size = Int64.Parse(m_sReply.Substring(4))
            Else
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            Return size
        End Function


        'Log on to the FTP server.
        Public Function Login() As Boolean

            'm_oEventLogging.LogThisEvent("Login - Start", EventLogEntryType.SuccessAudit)

            m_objClientSocket = _
            New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

            m_objClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_iClientSocketTimeout)

            Dim ep As New IPEndPoint(Dns.Resolve(m_sRemoteHost).AddressList(0), m_iRemotePort)

            Try
                m_objClientSocket.Connect(ep)
            Catch ex As Exception
                MessageString = m_sReply
                Throw New IOException("Cannot connect to the remote server - """ & ex.Message & """")

            End Try

            ReadReply()

            If (m_iRetValue <> 220) Then
                CloseConnection()
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If
            'Send an FTP command to send a user logon ID to the server.
            SendCommand("USER " & m_sRemoteUser)
            If (Not (m_iRetValue = 331 Or m_iRetValue = 230)) Then
                Cleanup()
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            If (m_iRetValue <> 230) Then
                'Send an FTP command to send a user logon password to the server.
                SendCommand("PASS " & m_sRemotePassword)
                If (Not (m_iRetValue = 230 Or m_iRetValue = 202)) Then
                    Cleanup()
                    MessageString = m_sReply
                    Throw New IOException(m_sReply.Substring(4))
                End If
            End If

            m_bLoggedIn = True
            'Call the ChangeDirectory user-defined function to change the directory to the 
            'remote FTP folder that is mapped.
            ChangeDirectory(m_sRemotePath)

            'Return the final result.
            'm_oEventLogging.LogThisEvent("Login - Done", EventLogEntryType.SuccessAudit)

            Return m_bLoggedIn
        End Function

        'If the value of mode is true, set binary mode for downloads. Otherwise, set ASCII mode.
        Public Sub SetBinaryMode(ByVal bMode As Boolean)

            If (bMode) Then
                'Send the FTP command to set the binary mode.
                '(TYPE is an FTP command that is used to specify representation type.)
                SendCommand("TYPE I")
            Else
                'Send the FTP command to set ASCII mode. 
                '(TYPE is an FTP command that is used to specify representation type.)
                SendCommand("TYPE A")
            End If

            If (m_iRetValue <> 200) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If
        End Sub
        ' Download a file to the local directory of the assembly. Keep the same file name.
        Public Sub DownloadFile(ByVal sFileName As String)
            DownloadFile(sFileName, "", False)
        End Sub
        ' Download a remote file to the local directory of the Assembly. Keep the same file name.
        Public Sub DownloadFile(ByVal sFileName As String, _
                                ByVal bResume As Boolean)
            DownloadFile(sFileName, "", bResume)
        End Sub
        'Download a remote file to a local file name. You must include a path.
        'The local file name will be created or will be overwritten, but the path must exist.
        Public Sub DownloadFile(ByVal sFileName As String, _
                                ByVal sLocalFileName As String)
            DownloadFile(sFileName, sLocalFileName, False)
        End Sub
        ' Download a remote file to a local file name. You must include a path. Set the 
        ' resume flag. The local file name will be created or will be overwritten, but the path must exist.
        Public Sub DownloadFile(ByVal sFileName As String, _
                                ByVal sLocalFileName As String, _
                                ByVal bResume As Boolean)
            Dim st As Stream
            Dim output As FileStream
            Dim cSocket As Socket
            Dim offset, npos As Long

            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("DownloadFile - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If

            SetBinaryMode(True)

            If (sLocalFileName.Equals("")) Then
                sLocalFileName = sFileName
            End If

            If (Not (File.Exists(sLocalFileName))) Then
                st = File.Create(sLocalFileName)
                st.Close()
            End If

            output = New FileStream(sLocalFileName, FileMode.Open)
            cSocket = CreateDataSocket()
            offset = 0

            If (bResume) Then
                offset = output.Length

                If (offset > 0) Then
                    'Send an FTP command to restart.
                    SendCommand("REST " & offset)
                    If (m_iRetValue <> 350) Then
                        offset = 0
                    End If
                End If

                If (offset > 0) Then
                    npos = output.Seek(offset, SeekOrigin.Begin)
                End If
            End If
            'Send an FTP command to retrieve a file.
            SendCommand("RETR " & sFileName)

            If (Not (m_iRetValue = 150 Or m_iRetValue = 125)) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            Do While (True)
                m_aBuffer.Clear(m_aBuffer, 0, m_aBuffer.Length)
                m_iBytes = cSocket.Receive(m_aBuffer, m_aBuffer.Length, 0)
                output.Write(m_aBuffer, 0, m_iBytes)

                If (m_iBytes <= 0) Then
                    Exit Do
                End If
            Loop

            output.Close()
            If (cSocket.Connected) Then
                cSocket.Close()
            End If

            ReadReply()
            If (Not (m_iRetValue = 226 Or m_iRetValue = 250)) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

        End Sub
        'This is a function that is used to upload a file from your local hard disk to your FTP site.
        Public Sub UploadFile(ByVal sFileName As String, ByVal sDestFileName As String)
            UploadFile(sFileName, sDestFileName, False)
        End Sub
        ' This is a function that is used to upload a file from your local hard disk to your FTP site 
        ' and then set the resume flag.
        Public Sub UploadFile(ByVal sFileName As String, ByVal sDestFileName As String, _
                              ByVal bResume As Boolean)
            ' make sure the file is there first
            'm_oEventLogging.LogThisEvent("UploadFile a - 1", EventLogEntryType.SuccessAudit)

            If Not File.Exists(sFileName) Then
                'm_oEventLogging.LogThisEvent("UploadFile a - 2", EventLogEntryType.SuccessAudit)
                MessageString = m_sReply
                Throw New IOException("The file: " & sFileName & " was not found. " & _
                    "Cannot upload the file to the FTP site")
            End If
            'm_oEventLogging.LogThisEvent("UploadFile a - 3", EventLogEntryType.SuccessAudit)

            Dim oFileStream As FileStream
            Try
                'm_oEventLogging.LogThisEvent("UploadFile a - 4", EventLogEntryType.SuccessAudit)
                oFileStream = New FileStream(sFileName, FileMode.Open)
                'm_oEventLogging.LogThisEvent("UploadFile a - 5", EventLogEntryType.SuccessAudit)
                UploadFile(oFileStream, sDestFileName, bResume)
                'm_oEventLogging.LogThisEvent("UploadFile a - 6", EventLogEntryType.SuccessAudit)
            Finally
                'm_oEventLogging.LogThisEvent("UploadFile a - 7", EventLogEntryType.SuccessAudit)
                Try : oFileStream.Close() : Catch ex As Exception : End Try
                'm_oEventLogging.LogThisEvent("UploadFile a - 8", EventLogEntryType.SuccessAudit)
            End Try
            'm_oEventLogging.LogThisEvent("UploadFile a - 9", EventLogEntryType.SuccessAudit)
        End Sub
        Public Sub SendFTPCommand(ByVal p_sFTPCommand As String)
            SendCommand(p_sFTPCommand)
            'm_oOculet.m_oEventLogging.LogThisEvent("SendFTPCommand reply: " & m_sReply, EventLogEntryType.Information)
            If (m_iRetValue >= 300) Then
                Throw New IOException(m_sReply.Substring(4))
            End If
        End Sub
        Public Sub UploadFile(ByVal oSourceStream As IO.Stream, ByVal sDestFileName As String, _
                              ByVal bResume As Boolean)
            Dim cSocket As Socket
            Dim offset As Long
            Dim bFileNotFound As Boolean

            'm_oEventLogging.LogThisEvent("UploadFile b - 1", EventLogEntryType.SuccessAudit)

            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("UploadFile - Login", EventLogEntryType.SuccessAudit)
                Login()
                'm_oEventLogging.LogThisEvent("UploadFile b - 3", EventLogEntryType.SuccessAudit)
            End If

            'm_oEventLogging.LogThisEvent("UploadFile b - 4", EventLogEntryType.SuccessAudit)
            cSocket = CreateDataSocket()
            cSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, m_iClientSocketTimeout)
            'm_oEventLogging.LogThisEvent("UploadFile b - 5", EventLogEntryType.SuccessAudit)

            offset = 0

            If (bResume) Then
                Try
                    'm_oEventLogging.LogThisEvent("UploadFile b - 6", EventLogEntryType.SuccessAudit)
                    SetBinaryMode(True)
                    offset = GetFileSize(sDestFileName)
                    'm_oEventLogging.LogThisEvent("UploadFile b - 7", EventLogEntryType.SuccessAudit)
                Catch ex As Exception
                    'm_oEventLogging.LogThisEvent("UploadFile b - 8", EventLogEntryType.SuccessAudit)
                    offset = 0
                End Try
            End If

            If (offset > 0) Then
                'm_oEventLogging.LogThisEvent("UploadFile b - 9", EventLogEntryType.SuccessAudit)
                SendCommand("REST " & offset)
                'm_oEventLogging.LogThisEvent("UploadFile b - 10", EventLogEntryType.SuccessAudit)
                If (m_iRetValue <> 350) Then
                    'm_oEventLogging.LogThisEvent("UploadFile b - 11", EventLogEntryType.SuccessAudit)

                    'The remote server may not support resuming.
                    offset = 0
                End If
            End If
            'Send an FTP command to store a file.
            'm_oEventLogging.LogThisEvent("UploadFile b - 12", EventLogEntryType.SuccessAudit)
            SendCommand("STOR " & sDestFileName)
            'm_oEventLogging.LogThisEvent("UploadFile b - 13", EventLogEntryType.SuccessAudit)
            If (Not (m_iRetValue = 125 Or m_iRetValue = 150)) Then
                'm_oEventLogging.LogThisEvent("UploadFile b - 14", EventLogEntryType.SuccessAudit)
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            'm_oEventLogging.LogThisEvent("UploadFile b - 15", EventLogEntryType.SuccessAudit)
            'Check to see if the file exists before the upload.
            ' Open the input stream to read the source file.
            If (offset <> 0) Then
                'm_oEventLogging.LogThisEvent("UploadFile b - 16", EventLogEntryType.SuccessAudit)
                oSourceStream.Seek(offset, SeekOrigin.Begin)
                'm_oEventLogging.LogThisEvent("UploadFile b - 17", EventLogEntryType.SuccessAudit)
            End If

            'm_oEventLogging.LogThisEvent("UploadFile b - 18", EventLogEntryType.SuccessAudit)

            'Upload the file. 
            m_iBytes = oSourceStream.Read(m_aBuffer, 0, m_aBuffer.Length)
            Do While (m_iBytes > 0)
                'm_oEventLogging.LogThisEvent("UploadFile b - 19", EventLogEntryType.SuccessAudit)
                cSocket.Send(m_aBuffer, m_iBytes, 0)
                m_iBytes = oSourceStream.Read(m_aBuffer, 0, m_aBuffer.Length)
                'm_oEventLogging.LogThisEvent("UploadFile b - 20", EventLogEntryType.SuccessAudit)
            Loop

            'm_oEventLogging.LogThisEvent("UploadFile b - 21", EventLogEntryType.SuccessAudit)
            ' do not close the stream here, let the caller handle that

            If (cSocket.Connected) Then
                'm_oEventLogging.LogThisEvent("UploadFile b - 22", EventLogEntryType.SuccessAudit)
                cSocket.Close()
                'm_oEventLogging.LogThisEvent("UploadFile b - 23", EventLogEntryType.SuccessAudit)
            End If

            'm_oEventLogging.LogThisEvent("UploadFile b - 24", EventLogEntryType.SuccessAudit)
            ReadReply()
            'm_oEventLogging.LogThisEvent("UploadFile b - 25", EventLogEntryType.SuccessAudit)
            If (Not (m_iRetValue = 226 Or m_iRetValue = 250)) Then
                'm_oEventLogging.LogThisEvent("UploadFile b - 26", EventLogEntryType.SuccessAudit)
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If
        End Sub
        ' Delete a file from the remote FTP server.
        Public Function DeleteFile(ByVal sFileName As String) As Boolean
            Dim bResult As Boolean

            bResult = True
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("DeleteFile - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command to delete a file.
            SendCommand("DELE " & sFileName)
            If (m_iRetValue <> 250) Then
                bResult = False
                MessageString = m_sReply
            End If

            ' Return the final result.
            Return bResult
        End Function
        ' Rename a file on the remote FTP server.
        Public Function RenameFile(ByVal sOldFileName As String, _
                                   ByVal sNewFileName As String) As Boolean
            Dim bResult As Boolean

            bResult = True
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("RenameFile - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command to rename from a file.
            SendCommand("RNFR " & sOldFileName)
            If (m_iRetValue <> 350) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If

            'Send an FTP command to rename a file to a new file name.
            'It will overwrite if newFileName exists. 
            SendCommand("RNTO " & sNewFileName)
            If (m_iRetValue <> 250) Then
                MessageString = m_sReply
                Throw New IOException(m_sReply.Substring(4))
            End If
            ' Return the final result.
            Return bResult
        End Function

        'This is a function that is used to create a directory on the remote FTP server.
        Public Function CreateDirectory(ByVal sDirName As String) As Boolean
            Dim bResult As Boolean

            bResult = True
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("CreateDirectory - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command to make directory on the FTP server.
            SendCommand("MKD " & sDirName)
            If (m_iRetValue <> 257) Then
                bResult = False
                MessageString = m_sReply
            End If

            ' Return the final result.
            Return bResult
        End Function
        ' This is a function that is used to delete a directory on the remote FTP server.
        Public Function RemoveDirectory(ByVal sDirName As String) As Boolean
            Dim bResult As Boolean

            bResult = True
            'Check if logged on to the FTP server
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("RemoveDirectory - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command to remove directory on the FTP server.
            SendCommand("RMD " & sDirName)
            If (m_iRetValue <> 250) Then
                bResult = False
                MessageString = m_sReply
            End If

            ' Return the final result.
            Return bResult
        End Function
        'This is a function that is used to change the current working directory on the remote FTP server.
        Public Function ChangeDirectory(ByVal sDirName As String) As Boolean
            Dim bResult As Boolean

            bResult = True
            'Check if you are in the root directory.
            If (sDirName.Equals(".")) Then
                Exit Function
            End If
            'Check if logged on to the FTP server
            If (Not (m_bLoggedIn)) Then
                'm_oEventLogging.LogThisEvent("ChangeDirectory - Login", EventLogEntryType.SuccessAudit)
                Login()
            End If
            'Send an FTP command to change directory on the FTP server.
            SendCommand("CWD " & sDirName)
            If (m_iRetValue <> 250) Then
                bResult = False
                MessageString = m_sReply
            End If

            Me.m_sRemotePath = sDirName

            ' Return the final result.
            Return bResult
        End Function
        ' Close the FTP connection of the remote server.
        Public Sub CloseConnection()
            If (Not (m_objClientSocket Is Nothing)) Then
                'Send an FTP command to end an FTP server system.
                SendCommand("QUIT")
            End If

            Cleanup()
        End Sub

#End Region

#Region "Private Subs and Functions"
        ' Read the reply from the FTP server.
        Private Sub ReadReply()
            m_sMes = ""
            m_sReply = ReadLine()
            Dim l_sStatusCode As String = Left(m_sReply, InStr(m_sReply, " ")).Trim
            Select Case l_sStatusCode
                Case "501", "502", "503", "504", "530", "532", "550", "552", "553", "10054", "10060", "10061", "10066", "10068"
                    Throw New Exception("Error: " & m_sReply)
            End Select

            m_iRetValue = Int32.Parse(m_sReply.Substring(0, 3))
        End Sub

        ' Clean up some variables.
        Private Sub Cleanup()
            If Not (m_objClientSocket Is Nothing) Then
                m_objClientSocket.Close()
                m_objClientSocket = Nothing
            End If

            m_bLoggedIn = False
        End Sub
        ' Read a line from the FTP server.
        Private Function ReadLine(Optional ByVal bClearMes As Boolean = False) As String
            Dim seperator As Char = ControlChars.Lf
            Dim mess() As String

            If (bClearMes) Then
                m_sMes = ""
            End If
            Do While (True)
                m_aBuffer.Clear(m_aBuffer, 0, BLOCK_SIZE)
                m_iBytes = m_objClientSocket.Receive(m_aBuffer, m_aBuffer.Length, 0)
                m_sMes += ASCII.GetString(m_aBuffer, 0, m_iBytes)
                If (m_iBytes < m_aBuffer.Length) Then
                    Exit Do
                End If
            Loop

            mess = m_sMes.Split(seperator)
            If (m_sMes.Length > 2) Then
                m_sMes = mess(mess.Length - 2)
            Else
                m_sMes = mess(0)
            End If

            If (Not (m_sMes.Substring(3, 1).Equals(" "))) Then
                Return ReadLine(True)
            End If

            Return m_sMes
        End Function
        ' This is a function that is used to send a command to the FTP server that you are connected to.
        Private Sub SendCommand(ByVal sCommand As String)
            sCommand = sCommand & ControlChars.CrLf
            Dim cmdbytes As Byte() = ASCII.GetBytes(sCommand)
            m_objClientSocket.Send(cmdbytes, cmdbytes.Length, 0)
            ReadReply()
        End Sub
        ' Create a data socket.
        Private Function CreateDataSocket() As Socket
            Dim index1, index2, len As Int32
            Dim partCount, i, port As Int32
            Dim ipData, buf, ipAddress As String
            Dim parts(6) As Int32
            Dim ch As Char
            Dim s As Socket
            Dim ep As IPEndPoint

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 1", EventLogEntryType.SuccessAudit)

            'Send an FTP command to use passive data connection. 
            SendCommand("PASV")
            If (m_iRetValue <> 227) Then
                MessageString = m_sReply
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 2 : " & m_sReply & " - " & m_iRetValue.ToString(), EventLogEntryType.SuccessAudit)
                Throw New IOException(m_sReply.Substring(4))
            End If

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 3 : " & m_sReply, EventLogEntryType.SuccessAudit)

            Dim altString As String = "227 Data transfer will passively listen to "

            If m_sReply.StartsWith(altString) Then
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 3a : " & m_sReply.Trim().Substring(altString.Length), EventLogEntryType.SuccessAudit)
                ipData = m_sReply.Trim().Substring(altString.Length)
            Else
                index1 = m_sReply.IndexOf("(")
                index2 = m_sReply.IndexOf(")")
                ipData = m_sReply.Substring(index1 + 1, index2 - index1 - 1)
            End If


            'm_oEventLogging.LogThisEvent("CreateDataSocket - 4", EventLogEntryType.SuccessAudit)
            len = ipData.Length
            partCount = 0
            buf = ""

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 5", EventLogEntryType.SuccessAudit)
            For i = 0 To ((len - 1) And CInt(partCount <= 6))
                ch = Char.Parse(ipData.Substring(i, 1))
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 6", EventLogEntryType.SuccessAudit)
                If (Char.IsDigit(ch)) Then
                    'm_oEventLogging.LogThisEvent("CreateDataSocket - 7", EventLogEntryType.SuccessAudit)
                    buf += ch
                ElseIf (ch <> ",") Then
                    'm_oEventLogging.LogThisEvent("CreateDataSocket - 8", EventLogEntryType.SuccessAudit)
                    MessageString = m_sReply
                    Throw New IOException("Malformed PASV reply: " & m_sReply)
                End If

                If ((ch = ",") Or (i + 1 = len)) Then
                    Try
                        'm_oEventLogging.LogThisEvent("CreateDataSocket - 9", EventLogEntryType.SuccessAudit)
                        parts(partCount) = Int32.Parse(buf)
                        partCount += 1
                        buf = ""
                        'm_oEventLogging.LogThisEvent("CreateDataSocket - 10", EventLogEntryType.SuccessAudit)
                    Catch ex As Exception
                        'm_oEventLogging.LogThisEvent("CreateDataSocket - 11", EventLogEntryType.SuccessAudit)
                        MessageString = m_sReply
                        Throw New IOException("Malformed PASV reply: " & m_sReply)
                    End Try
                End If
            Next

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 12", EventLogEntryType.SuccessAudit)
            ipAddress = parts(0) & "." & parts(1) & "." & parts(2) & "." & parts(3)

            ' Make this call in Visual Basic .NET 2002. You want to 
            ' bitshift the number by 8 bits. In Visual Basic .NET 2002 you must
            ' multiply the number by 2 to the power of 8.
            'port = parts(4) * (2 ^ 8)

            ' Make this call and then comment out the previous line for Visual Basic .NET 2003.
            port = parts(4) << 8

            ' Determine the data port number.
            port = port + parts(5)

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 13", EventLogEntryType.SuccessAudit)

            s = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ep = New IPEndPoint(Dns.Resolve(ipAddress).AddressList(0), port)

            'm_oEventLogging.LogThisEvent("CreateDataSocket - 14", EventLogEntryType.SuccessAudit)

            Try
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 15", EventLogEntryType.SuccessAudit)
                s.Connect(ep)
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 16", EventLogEntryType.SuccessAudit)
            Catch ex As Exception
                'm_oEventLogging.LogThisEvent("CreateDataSocket - 17", EventLogEntryType.SuccessAudit)
                MessageString = m_sReply
                Throw New IOException("Cannot connect to remote server.")
                'If you cannot connect to the FTP server that is 
                'specified, make the boolean variable false.
                flag_bool = False
            End Try
            'If you can connect to the FTP server that is specified, make the boolean variable true.
            flag_bool = True
            'm_oEventLogging.LogThisEvent("CreateDataSocket - 18", EventLogEntryType.SuccessAudit)
            Return s
        End Function

#End Region

    End Class

    Public Class DirFile
        Implements IComparable

        Private _Name As String
        Private _IsDir As Boolean
        Private _Date As String
        Private _Time As String
        Private _Size As Integer

        Public ReadOnly Property Name() As String
            Get
                Return _Name
            End Get
        End Property

        Public ReadOnly Property IsDir() As Boolean
            Get
                Return _IsDir
            End Get
        End Property

        Public ReadOnly Property FileDate() As String
            Get
                Return _Date
            End Get
        End Property

        Public ReadOnly Property FileTime() As String
            Get
                Return _Time
            End Get
        End Property

        Public ReadOnly Property FileSize() As Integer
            Get
                Return _Size
            End Get
        End Property

        Public Sub New(ByVal Name As String, ByVal FDate As String, ByVal FTime As String, Optional ByVal Dir As Boolean = False, Optional ByVal FSize As Integer = 0)
            _Name = Name
            _Date = FDate
            _Time = FTime
            _IsDir = Dir
            _Size = FSize
        End Sub

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            Dim df As DirFile

            ' Any non-Nothing object is greater than nothing. 
            If obj Is Nothing Then
                Return 1
            End If

            ' Avoid late-binding and cast to a specific Person Object. 
            df = CType(obj, DirFile)

            ' Use the String's CompareTo Method to perform the check. 
            Return Me.IsDir.CompareTo(df.IsDir)

        End Function
    End Class

    Public Class DirList
        Inherits CollectionBase

        ' Create the Default Property Item for this collection. 
        ' Allow the retrieval by index. 
        Default ReadOnly Property Item(ByVal index As Int32) As DirFile
            Get
                ' Avoid Late-binding and return this as a DirFile Object 
                Return CType(Me.InnerList.Item(index), DirFile)
            End Get
        End Property

        ' Create another default property for this collection. 
        ' Allow retrieval by name. 
        Default ReadOnly Property Item(ByVal Instance As String) As DirFile
            Get
                Dim df As DirFile
                For Each df In Me.InnerList
                    If df.Name = Instance Then
                        ' We found our DirFile object, return it. 
                        Return df
                    End If
                Next
            End Get
        End Property

        ' Create another property for this collection. 
        ' Allows checking for existance by name. 
        ReadOnly Property Exists(ByVal Instance As String) As Boolean
            Get
                Dim df As DirFile
                For Each df In Me.InnerList
                    If df.Name = Instance Then
                        ' We found our DirFile object, return it. 
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

        ' Create a new instance of our Collection, by calling MyBase.New 
        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            If Not TypeOf newValue Is DirFile Then
                Throw New ArgumentException(String.Format("Cannot add a {0} type to this collection", newValue.GetType.ToString), "Value")
            End If
        End Sub

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As Object)
            If Not TypeOf value Is DirFile Then
                Throw New ArgumentException(String.Format("Cannot add a {0} type to this collection", value.GetType.ToString), "Value")
            End If
        End Sub

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As Object)
            If Not TypeOf value Is DirFile Then
                Throw New ArgumentException(String.Format("Cannot add a {0} type to this collection", value.GetType.ToString), "Value")
            End If
        End Sub

        ' Method to add a single DirFile object to our collection. 
        Public Function Add(ByVal value As DirFile) As Int32
            Return Me.InnerList.Add(value)
        End Function

        ' Method to Remove a specific DirFile object from our collection. 
        Public Sub Remove(ByVal value As DirFile)
            Me.InnerList.Remove(value)
        End Sub

        ' Method to check if a DirFile object already exists in the collection. 
        Public Function Contains(ByVal value As DirFile) As Boolean
            Return Me.Contains(value)
        End Function

        ' Create the Sort method for the collection. 
        Public Sub Sort()
            Me.InnerList.Sort()
        End Sub

    End Class
End Namespace
