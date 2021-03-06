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

  public partial class TTypeEntry : TBase
  {
    private TPrimitiveTypeEntry _primitiveEntry;
    private TArrayTypeEntry _arrayEntry;
    private TMapTypeEntry _mapEntry;
    private TStructTypeEntry _structEntry;
    private TUnionTypeEntry _unionEntry;
    private TUserDefinedTypeEntry _userDefinedTypeEntry;

    public TPrimitiveTypeEntry PrimitiveEntry
    {
      get
      {
        return _primitiveEntry;
      }
      set
      {
        __isset.primitiveEntry = true;
        this._primitiveEntry = value;
      }
    }

    public TArrayTypeEntry ArrayEntry
    {
      get
      {
        return _arrayEntry;
      }
      set
      {
        __isset.arrayEntry = true;
        this._arrayEntry = value;
      }
    }

    public TMapTypeEntry MapEntry
    {
      get
      {
        return _mapEntry;
      }
      set
      {
        __isset.mapEntry = true;
        this._mapEntry = value;
      }
    }

    public TStructTypeEntry StructEntry
    {
      get
      {
        return _structEntry;
      }
      set
      {
        __isset.structEntry = true;
        this._structEntry = value;
      }
    }

    public TUnionTypeEntry UnionEntry
    {
      get
      {
        return _unionEntry;
      }
      set
      {
        __isset.unionEntry = true;
        this._unionEntry = value;
      }
    }

    public TUserDefinedTypeEntry UserDefinedTypeEntry
    {
      get
      {
        return _userDefinedTypeEntry;
      }
      set
      {
        __isset.userDefinedTypeEntry = true;
        this._userDefinedTypeEntry = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool primitiveEntry;
      public bool arrayEntry;
      public bool mapEntry;
      public bool structEntry;
      public bool unionEntry;
      public bool userDefinedTypeEntry;
    }

    public TTypeEntry()
    {
    }

    public async Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
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
                PrimitiveEntry = new TPrimitiveTypeEntry();
                await PrimitiveEntry.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.Struct)
              {
                ArrayEntry = new TArrayTypeEntry();
                await ArrayEntry.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 3:
              if (field.Type == TType.Struct)
              {
                MapEntry = new TMapTypeEntry();
                await MapEntry.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 4:
              if (field.Type == TType.Struct)
              {
                StructEntry = new TStructTypeEntry();
                await StructEntry.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 5:
              if (field.Type == TType.Struct)
              {
                UnionEntry = new TUnionTypeEntry();
                await UnionEntry.ReadAsync(iprot, cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 6:
              if (field.Type == TType.Struct)
              {
                UserDefinedTypeEntry = new TUserDefinedTypeEntry();
                await UserDefinedTypeEntry.ReadAsync(iprot, cancellationToken);
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
        var struc = new TStruct("TTypeEntry");
        await oprot.WriteStructBeginAsync(struc, cancellationToken);
        var field = new TField();
        if (PrimitiveEntry != null && __isset.primitiveEntry)
        {
          field.Name = "primitiveEntry";
          field.Type = TType.Struct;
          field.ID = 1;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await PrimitiveEntry.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (ArrayEntry != null && __isset.arrayEntry)
        {
          field.Name = "arrayEntry";
          field.Type = TType.Struct;
          field.ID = 2;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await ArrayEntry.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (MapEntry != null && __isset.mapEntry)
        {
          field.Name = "mapEntry";
          field.Type = TType.Struct;
          field.ID = 3;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await MapEntry.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (StructEntry != null && __isset.structEntry)
        {
          field.Name = "structEntry";
          field.Type = TType.Struct;
          field.ID = 4;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await StructEntry.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (UnionEntry != null && __isset.unionEntry)
        {
          field.Name = "unionEntry";
          field.Type = TType.Struct;
          field.ID = 5;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await UnionEntry.WriteAsync(oprot, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if (UserDefinedTypeEntry != null && __isset.userDefinedTypeEntry)
        {
          field.Name = "userDefinedTypeEntry";
          field.Type = TType.Struct;
          field.ID = 6;
          await oprot.WriteFieldBeginAsync(field, cancellationToken);
          await UserDefinedTypeEntry.WriteAsync(oprot, cancellationToken);
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
      var sb = new StringBuilder("TTypeEntry(");
      bool __first = true;
      if (PrimitiveEntry != null && __isset.primitiveEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("PrimitiveEntry: ");
        sb.Append(PrimitiveEntry== null ? "<null>" : PrimitiveEntry.ToString());
      }
      if (ArrayEntry != null && __isset.arrayEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("ArrayEntry: ");
        sb.Append(ArrayEntry== null ? "<null>" : ArrayEntry.ToString());
      }
      if (MapEntry != null && __isset.mapEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("MapEntry: ");
        sb.Append(MapEntry== null ? "<null>" : MapEntry.ToString());
      }
      if (StructEntry != null && __isset.structEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("StructEntry: ");
        sb.Append(StructEntry== null ? "<null>" : StructEntry.ToString());
      }
      if (UnionEntry != null && __isset.unionEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("UnionEntry: ");
        sb.Append(UnionEntry== null ? "<null>" : UnionEntry.ToString());
      }
      if (UserDefinedTypeEntry != null && __isset.userDefinedTypeEntry)
      {
        if(!__first) { sb.Append(", "); }
        __first = false;
        sb.Append("UserDefinedTypeEntry: ");
        sb.Append(UserDefinedTypeEntry== null ? "<null>" : UserDefinedTypeEntry.ToString());
      }
      sb.Append(")");
      return sb.ToString();
    }
  }

}
