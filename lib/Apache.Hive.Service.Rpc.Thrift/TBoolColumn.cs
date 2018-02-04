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

  public partial class TBoolColumn : TBase
  {

    public List<bool> Values { get; set; }

    public byte[] Nulls { get; set; }

    public TBoolColumn()
    {
    }

    public TBoolColumn(List<bool> values, byte[] nulls) : this()
    {
      this.Values = values;
      this.Nulls = nulls;
    }

    public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_values = false;
        bool isset_nulls = false;
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
              if (field.Type == TType.List)
              {
                {
                  Values = new List<bool>();
                  TList _list27 = await iprot.ReadListBeginAsync(cancellationToken);
                  for(int _i28 = 0; _i28 < _list27.Count; ++_i28)
                  {
                    bool _elem29;
                    _elem29 = await iprot.ReadBoolAsync(cancellationToken);
                    Values.Add(_elem29);
                  }
                  await iprot.ReadListEndAsync(cancellationToken);
                }
                isset_values = true;
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.String)
              {
                Nulls = await iprot.ReadBinaryAsync(cancellationToken);
                isset_nulls = true;
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
        if (!isset_values)
        {
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        }
        if (!isset_nulls)
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
        var struc = new TStruct("TBoolColumn");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        field.Name = "values";
        field.Type = TType.List;
        field.ID = 1;
        await oprot.WriteFieldBeginAsync(field, cancellationToken);
        {
          await oprot.WriteListBeginAsync(new TList(TType.Bool, Values.Count), cancellationToken);
          foreach (bool _iter30 in Values)
          {
            await oprot.WriteBoolAsync(_iter30, cancellationToken);
          }
          await oprot.WriteListEndAsync(cancellationToken);
        }
        await oprot.WriteFieldEndAsync(cancellationToken);
        field.Name = "nulls";
        field.Type = TType.String;
        field.ID = 2;
        await oprot.WriteFieldBeginAsync(field, cancellationToken);
        await oprot.WriteBinaryAsync(Nulls, cancellationToken);
        await oprot.WriteFieldEndAsync(cancellationToken);
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
      var sb = new StringBuilder("TBoolColumn(");
      sb.Append(", Values: ");
      sb.Append(Values);
      sb.Append(", Nulls: ");
      sb.Append(Nulls);
      sb.Append(")");
      return sb.ToString();
    }
  }

}