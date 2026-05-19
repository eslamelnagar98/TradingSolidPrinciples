namespace SD_PD_IDA_46.OCP;

// ╔══════════════════════════════════════════════════════════════════════╗
//  OCP — ILogger  (the CLOSED contract)
//
//  This interface is the stable abstraction that will never change.
//  Adding a new logging destination (file, console, cloud, database...)
//  means adding a NEW class that implements this interface.
//  This interface itself is NEVER modified.
//
//  OCP Rule applied:
//   ✔  CLOSED for modification — this contract never changes.
//   ✔  OPEN for extension     — new loggers implement it freely.
// ╚══════════════════════════════════════════════════════════════════════╝
public interface ILogger
{
    void LogInfo(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogException(string message, Exception exception);
}
