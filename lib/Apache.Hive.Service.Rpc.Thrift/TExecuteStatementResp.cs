/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Collections;

using Thrift.Protocols;
using Thrift.Protocols.Entities;
using Thrift.Protocols.Utilities;
using Thrift.Transports;
using Thrift.Transports.Client;
using Thrift.Transports.Server;


namespace Apache.Hive.Service.Rpc.Thrift
{

  public partial class TExecuteStatementResp : TBase
  {
    private TOperationHandle _operationHandle;

    public TStatus Status { get; set; }

    public TOperationHandle OperationHandle
    {
      get
      {
        return _operationHandle;
      }
      set
      {
        __isset.operationHandle = true;
        this._operationHandle = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool operationHandle;
    }

    public TExecuteStatementResp()
    {
    }

    public TExecuteStatementResp(TStatus status) : this()
    {
      this.Status = status;
    }

    public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_status = false;
        TField field;
        await iprot.ReadStructBeginAsync(cancellationToken);
        while (true)
        {
          field = await iprot.ReadFieldBeginAsync(cancellationToken);
          if (field.Type == TType.Stop)
          {
            break;
          }

          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct)
              {
                Status = new TStatus();
                await Status.ReadAsync(iprot, cancellationToken);
                isset_status = true;
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.Struct)
              {
                OperationHandle = new TOperationHandle();
                await OperationHandle.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            default: 
              await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              break;
          }

          await iprot.ReadFieldEndAsync(cancellationToken);
        }

        await iprot.ReadStructEndAsync(cancellationToken);
        if (!isset_status)
        {
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        }
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public async Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
    {
      oprot.IncrementRecursionDepth();
      try
      {
        var struc = new TStruct("TExecuteStatementResp");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        field.Name = "status";
        field.Type = TType.Struct;
        field.ID = 1;
        await oprot.WriteFieldBeginAsync(field, cancellationToken);
        await Status.WriteAsync(oprot, cancellationToken);
        await oprot.WriteFieldEndAsync(cancellationToken);
        if (OperationHandle != null && __isset.operationHandle)
        {
          field.Name = "operationHandle";
          field.Type = TType.Struct;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await OperationHandle.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        await oprot.WriteFieldStopAsync(cancellationToken);
        await oprot.WriteStructEndAsync(cancellationToken);
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder("TExecuteStatementResp(");
      sb.Append(", Status: ");
      sb.Append(Status== null ? "<null>" : Status.ToString());
      if (OperationHandle != null && __isset.operationHandle)
      {
        sb.Append(", OperationHandle: ");
        sb.Append(OperationHandle== null ? "<null>" : OperationHandle.ToString());
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}
