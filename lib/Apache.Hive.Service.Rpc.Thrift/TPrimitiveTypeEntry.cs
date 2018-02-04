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

  public partial class TPrimitiveTypeEntry : TBase
  {
    private TTypeQualifiers _typeQualifiers;

    /// <summary>
    /// 
    /// <seealso cref="TTypeId"/>
    /// </summary>
    public TTypeId Type { get; set; }

    public TTypeQualifiers TypeQualifiers
    {
      get
      {
        return _typeQualifiers;
      }
      set
      {
        __isset.typeQualifiers = true;
        this._typeQualifiers = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool typeQualifiers;
    }

    public TPrimitiveTypeEntry()
    {
    }

    public TPrimitiveTypeEntry(TTypeId type) : this()
    {
      this.Type = type;
    }

    public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_type = false;
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
              if (field.Type == TType.I32)
              {
                Type = (TTypeId)await iprot.ReadI32Async(cancellationToken);
                isset_type = true;
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.Struct)
              {
                TypeQualifiers = new TTypeQualifiers();
                await TypeQualifiers.ReadAsync(iprot, cancellationToken);
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
        if (!isset_type)
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
        var struc = new TStruct("TPrimitiveTypeEntry");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 1;
        await oprot.WriteFieldBeginAsync(field, cancellationToken);
        await oprot.WriteI32Async((int)Type, cancellationToken);
        await oprot.WriteFieldEndAsync(cancellationToken);
        if (TypeQualifiers != null && __isset.typeQualifiers)
        {
          field.Name = "typeQualifiers";
          field.Type = TType.Struct;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await TypeQualifiers.WriteAsync(oprot, cancellationToken);
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
      var sb = new StringBuilder("TPrimitiveTypeEntry(");
      sb.Append(", Type: ");
      sb.Append(Type);
      if (TypeQualifiers != null && __isset.typeQualifiers)
      {
        sb.Append(", TypeQualifiers: ");
        sb.Append(TypeQualifiers== null ? "<null>" : TypeQualifiers.ToString());
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}
