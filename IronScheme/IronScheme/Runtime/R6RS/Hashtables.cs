#region License
/* ****************************************************************************
 * Copyright (c) Llewellyn Pritchard. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************/
#endregion

#if R6RS
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Generation;
using Microsoft.Scripting;
using System.Reflection;
using Microsoft.Scripting.Utils;
using System.Reflection.Emit;
using System.Collections;

namespace IronScheme.Runtime.R6RS
{
  public class Hashtables : Builtins
  {
    sealed class ReadOnlyHashtable : Hashtable
    {
      public ReadOnlyHashtable(IDictionary content)
      {
        foreach (DictionaryEntry de in content)
        {
          // verify this
          base.Add(de.Key, de.Value);
        }
      }

      public override void Clear()
      {
        throw new NotSupportedException("Hashtable is read-only");
      }

      public override void Add(object key, object value)
      {
        throw new NotSupportedException("Hashtable is read-only");
      }

      public override void Remove(object key)
      {
        throw new NotSupportedException("Hashtable is read-only");
      }

      public override object this[object key]
      {
        get
        {
          return base[key];
        }
        set
        {
          throw new NotSupportedException("Hashtable is read-only");
        }
      }
    }

    [Builtin("hashtable?")]
    public static object IsHashtable(object obj)
    {
      return obj is Hashtable;
    }

    [Builtin("hashtable-copy")]
    public static object HashtableCopy(object obj)
    {
      Hashtable ht = RequiresNotNull<Hashtable>(obj);
      return new Hashtable(ht);
    }

    [Builtin("hashtable-copy")]
    public static object HashtableCopy(object obj, object mutable)
    {
      Hashtable ht = RequiresNotNull<Hashtable>(obj);
      bool m = RequiresNotNull<bool>(mutable);
      if (m)
      {
        return new Hashtable(ht);
      }
      else
      {
        return new ReadOnlyHashtable(ht);
      }
    }

    [Builtin("hashtable-keys")]
    public static object HashtableKeys(object obj)
    {
      Hashtable ht = RequiresNotNull<Hashtable>(obj);
      ArrayList keys = new ArrayList(ht.Keys);
      return keys.ToArray();
    }

    [Builtin("hashtable-entries")]
    public static object HashtableEntries(object obj)
    {
      Hashtable ht = RequiresNotNull<Hashtable>(obj);
      ArrayList keys = new ArrayList();
      ArrayList values = new ArrayList();

      foreach (DictionaryEntry de in ht)
      {
        keys.Add(de.Key);
        values.Add(de.Value);
      }
      return Values((object)keys.ToArray(), (object)values.ToArray());
    }

    [Builtin("hashtable-mutable?")]
    public static object IsHashtableMutable(object obj)
    {
      RequiresNotNull<Hashtable>(obj);
      return !(obj is ReadOnlyHashtable);
    }

    [Builtin("equal-hash")]
    public static object EqualHash(object obj)
    {
      string r = WriteFormat(obj);
      return r.GetHashCode();
    }

    [Builtin("string-hash")]
    public static object StringHash(object obj)
    {
      string r = RequiresNotNull<string>(obj);
      return r.GetHashCode();
    }

    [Builtin("string-ci-hash")]
    public static object StringCaseInsensitiveHash(object obj)
    {
      //TODO: ci hash
      string r = RequiresNotNull<string>(obj);
      return r.GetHashCode();
    }

    [Builtin("symbol-hash")]
    public static object SymbolHash(object obj)
    {
      SymbolId s = RequiresNotNull<SymbolId>(obj);
      return s.GetHashCode();
    }
  }
}
#endif