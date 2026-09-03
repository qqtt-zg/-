
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.8.3928.0



// 警告 : 这是一个不完整的反汇编程序，不适合于重新汇编



.class public auto ansi beforefieldinit AntdUI.Select
       extends AntdUI.Input
       implements AntdUI.SubLayeredForm,
                  AntdUI.IEventListener
{
  .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 02 00 00 ) 
  .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
  .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 10 53 65 6C 65 63 74 20 E9 80 89 E6 8B A9   // ...Select ......
                                                                                              E5 99 A8 00 00 ) 
  .custom instance void [System]System.ComponentModel.ToolboxItemAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
  .custom instance void [System]System.ComponentModel.DefaultEventAttribute::.ctor(string) = ( 01 00 14 53 65 6C 65 63 74 65 64 49 6E 64 65 78   // ...SelectedIndex
                                                                                               43 68 61 6E 67 65 64 00 00 )                      // Changed..
  .interfaceimpl type AntdUI.SubLayeredForm
  .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
  .interfaceimpl type AntdUI.IEventListener
  .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
  .class auto ansi sealed nested public FilterEventHandler
         extends [mscorlib]System.MulticastDelegate
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
    .method public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    {
    } // end of method FilterEventHandler::.ctor

    .method public hidebysig newslot virtual 
            instance class [mscorlib]System.Collections.Generic.IList`1<object> 
            Invoke(object sender,
                   string 'value') runtime managed
    {
      .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
      .param [0]
      .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8[]) = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method FilterEventHandler::Invoke

    .method public hidebysig newslot virtual 
            instance class [mscorlib]System.IAsyncResult 
            BeginInvoke(object sender,
                        string 'value',
                        class [mscorlib]System.AsyncCallback callback,
                        object 'object') runtime managed
    {
      .param [1]
      .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
      .param [2]
      .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    } // end of method FilterEventHandler::BeginInvoke

    .method public hidebysig newslot virtual 
            instance class [mscorlib]System.Collections.Generic.IList`1<object> 
            EndInvoke(class [mscorlib]System.IAsyncResult result) runtime managed
    {
      .param [0]
      .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8[]) = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method FilterEventHandler::EndInvoke

  } // end of class FilterEventHandler

  .class auto ansi sealed nested private beforefieldinit '<>c__DisplayClass113_0'
         extends [mscorlib]System.Object
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    .field public class AntdUI.Select '<>4__this'
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
    .field public string temp
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
    .method public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    {
      // 代码大小       7 (0x7)
      .maxstack  8
      IL_0000:  ldarg.0
      IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
      IL_0006:  ret
    } // end of method '<>c__DisplayClass113_0'::.ctor

    .method assembly hidebysig instance void 
            '<OnTextChanged>b__0'() cil managed
    {
      // 代码大小       164 (0xa4)
      .maxstack  3
      .locals init (class [mscorlib]System.Collections.Generic.IList`1<object> V_0)
      IL_0000:  ldarg.0
      IL_0001:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_0006:  ldfld      class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
      IL_000b:  ldarg.0
      IL_000c:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_0011:  ldarg.0
      IL_0012:  ldfld      string AntdUI.Select/'<>c__DisplayClass113_0'::temp
      IL_0017:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.Select/FilterEventHandler::Invoke(object,
                                                                                                                                        string)
      IL_001c:  stloc.0
      IL_001d:  ldarg.0
      IL_001e:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_0023:  ldfld      string AntdUI.Select::filtertext
      IL_0028:  ldarg.0
      IL_0029:  ldfld      string AntdUI.Select/'<>c__DisplayClass113_0'::temp
      IL_002e:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                     string)
      IL_0033:  brfalse.s  IL_00a3

      IL_0035:  ldarg.0
      IL_0036:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_003b:  call       instance class AntdUI.BaseCollection AntdUI.Select::get_Items()
      IL_0040:  callvirt   instance void AntdUI.BaseCollection::Clear()
      IL_0045:  ldloc.0
      IL_0046:  brfalse.s  IL_0050

      IL_0048:  ldloc.0
      IL_0049:  callvirt   instance int32 class [mscorlib]System.Collections.Generic.ICollection`1<object>::get_Count()
      IL_004e:  brtrue.s   IL_0067

      IL_0050:  ldarg.0
      IL_0051:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_0056:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_005b:  dup
      IL_005c:  brtrue.s   IL_0060

      IL_005e:  pop
      IL_005f:  ret

      IL_0060:  ldc.i4.0
      IL_0061:  call       instance void AntdUI.ILayeredForm::IClose(bool)
      IL_0066:  ret

      IL_0067:  ldarg.0
      IL_0068:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_006d:  call       instance class AntdUI.BaseCollection AntdUI.Select::get_Items()
      IL_0072:  ldloc.0
      IL_0073:  callvirt   instance void AntdUI.BaseCollection::AddRange(class [mscorlib]System.Collections.Generic.IList`1<object>)
      IL_0078:  ldarg.0
      IL_0079:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_007e:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_0083:  brtrue.s   IL_0092

      IL_0085:  ldarg.0
      IL_0086:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_008b:  ldloc.0
      IL_008c:  call       instance void AntdUI.Select::ShowLayeredForm(class [mscorlib]System.Collections.Generic.IList`1<object>)
      IL_0091:  ret

      IL_0092:  ldarg.0
      IL_0093:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
      IL_0098:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_009d:  ldloc.0
      IL_009e:  callvirt   instance void AntdUI.LayeredFormSelectDown::TextChange(class [mscorlib]System.Collections.Generic.IList`1<object>)
      IL_00a3:  ret
    } // end of method '<>c__DisplayClass113_0'::'<OnTextChanged>b__0'

  } // end of class '<>c__DisplayClass113_0'

  .class auto ansi sealed nested private beforefieldinit '<>c__DisplayClass134_0'
         extends [mscorlib]System.Object
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    .field public class AntdUI.Select '<>4__this'
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
    .field public class [mscorlib]System.Collections.Generic.IList`1<object> list
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8[]) = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .method public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    {
      // 代码大小       7 (0x7)
      .maxstack  8
      IL_0000:  ldarg.0
      IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
      IL_0006:  ret
    } // end of method '<>c__DisplayClass134_0'::.ctor

    .method assembly hidebysig instance void 
            '<ShowLayeredForm>b__0'() cil managed
    {
      // 代码大小       18 (0x12)
      .maxstack  8
      IL_0000:  ldarg.0
      IL_0001:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
      IL_0006:  ldarg.0
      IL_0007:  ldfld      class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.Select/'<>c__DisplayClass134_0'::list
      IL_000c:  call       instance void AntdUI.Select::ShowLayeredForm(class [mscorlib]System.Collections.Generic.IList`1<object>)
      IL_0011:  ret
    } // end of method '<>c__DisplayClass134_0'::'<ShowLayeredForm>b__0'

    .method assembly hidebysig instance void 
            '<ShowLayeredForm>b__1'(object a,
                                    class [mscorlib]System.EventArgs b) cil managed
    {
      .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 00 00 00 ) 
      // 代码大小       49 (0x31)
      .maxstack  8
      IL_0000:  ldarg.0
      IL_0001:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
      IL_0006:  ldc.i4.0
      IL_0007:  stfld      int32 AntdUI.Select::select_x
      IL_000c:  ldarg.0
      IL_000d:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
      IL_0012:  ldnull
      IL_0013:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_0018:  ldarg.0
      IL_0019:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
      IL_001e:  ldc.i4.0
      IL_001f:  call       instance void AntdUI.Select::set_Expand(bool)
      IL_0024:  ldarg.0
      IL_0025:  ldfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
      IL_002a:  ldc.i4.0
      IL_002b:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
      IL_0030:  ret
    } // end of method '<>c__DisplayClass134_0'::'<ShowLayeredForm>b__1'

  } // end of class '<>c__DisplayClass134_0'

  .field private bool _list
  .field private valuetype AntdUI.TAlignFrom '<Placement>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<ListAutoWidth>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private int32 '<MaxCount>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private valuetype [mscorlib]System.Nullable`1<int32> '<DropDownRadius>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<DropDownArrow>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private valuetype [System.Drawing]System.Drawing.Size '<DropDownPadding>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private valuetype AntdUI.TAlign '<DropDownTextAlign>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<EnterDropDown>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<DropDownEmptyClose>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<ClickEnd>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<ClickSwitchDropdown>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<CloseIcon>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<Empty>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<AutoPrefixSvg>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<AutoText>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private bool '<WheelModifyEnabled>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.BaseCollection items
  .field private int32 selectedIndexX
  .field private int32 selectedIndex
  .field private object selectedValue
  .field private class AntdUI.SelectItem selectedItem
  .field private bool noex
  .field private class AntdUI.IntEventHandler SelectedIndexChanged
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.IntXYEventHandler SelectedIndexsChanged
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.ObjectNEventHandler SelectedValueChanged
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.BoolEventHandler ExpandDropChanged
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.ObjectNEventHandler ClosedItem
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private class AntdUI.Select/FilterEventHandler FilterChanged
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private string filtertext
  .field private bool showicon
  .field private class AntdUI.LayeredFormSelectDown subForm
  .field private class AntdUI.AnimationTask ThreadExpand
  .field private float32 ArrowProg
  .field private bool expand
  .field assembly int32 select_x
  .field private bool expandDrop
  .method family hidebysig specialname virtual 
          instance bool  get_BanInput() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::_list
    IL_0006:  ret
  } // end of method Select::get_BanInput

  .method public hidebysig specialname instance bool 
          get_List() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::_list
    IL_0006:  ret
  } // end of method Select::get_List

  .method public hidebysig specialname instance void 
          set_List(bool 'value') cil managed
  {
    // 代码大小       45 (0x2d)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::_list
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldarg.1
    IL_000c:  stfld      bool AntdUI.Select::_list
    IL_0011:  ldarg.0
    IL_0012:  ldfld      class AntdUI.Input/ICaret AntdUI.Input::CaretInfo
    IL_0017:  ldarg.1
    IL_0018:  stfld      bool AntdUI.Input/ICaret::ReadShow
    IL_001d:  ldarg.1
    IL_001e:  brfalse.s  IL_002c

    IL_0020:  ldarg.0
    IL_0021:  ldfld      class AntdUI.Input/ICaret AntdUI.Input::CaretInfo
    IL_0026:  ldc.i4.0
    IL_0027:  callvirt   instance void AntdUI.Input/ICaret::set_Show(bool)
    IL_002c:  ret
  } // end of method Select::set_List

  .method public hidebysig specialname instance valuetype AntdUI.TAlignFrom 
          get_Placement() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      valuetype AntdUI.TAlignFrom AntdUI.Select::'<Placement>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_Placement

  .method public hidebysig specialname instance void 
          set_Placement(valuetype AntdUI.TAlignFrom 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      valuetype AntdUI.TAlignFrom AntdUI.Select::'<Placement>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_Placement

  .method public hidebysig specialname instance bool 
          get_ListAutoWidth() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<ListAutoWidth>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_ListAutoWidth

  .method public hidebysig specialname instance void 
          set_ListAutoWidth(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<ListAutoWidth>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_ListAutoWidth

  .method public hidebysig specialname instance int32 
          get_MaxCount() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 AntdUI.Select::'<MaxCount>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_MaxCount

  .method public hidebysig specialname instance void 
          set_MaxCount(int32 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      int32 AntdUI.Select::'<MaxCount>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_MaxCount

  .method public hidebysig specialname instance valuetype [mscorlib]System.Nullable`1<int32> 
          get_DropDownRadius() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      valuetype [mscorlib]System.Nullable`1<int32> AntdUI.Select::'<DropDownRadius>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_DropDownRadius

  .method public hidebysig specialname instance void 
          set_DropDownRadius(valuetype [mscorlib]System.Nullable`1<int32> 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      valuetype [mscorlib]System.Nullable`1<int32> AntdUI.Select::'<DropDownRadius>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_DropDownRadius

  .method public hidebysig specialname instance bool 
          get_DropDownArrow() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<DropDownArrow>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_DropDownArrow

  .method public hidebysig specialname instance void 
          set_DropDownArrow(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<DropDownArrow>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_DropDownArrow

  .method public hidebysig specialname instance valuetype [System.Drawing]System.Drawing.Size 
          get_DropDownPadding() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      valuetype [System.Drawing]System.Drawing.Size AntdUI.Select::'<DropDownPadding>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_DropDownPadding

  .method public hidebysig specialname instance void 
          set_DropDownPadding(valuetype [System.Drawing]System.Drawing.Size 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      valuetype [System.Drawing]System.Drawing.Size AntdUI.Select::'<DropDownPadding>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_DropDownPadding

  .method public hidebysig specialname instance valuetype AntdUI.TAlign 
          get_DropDownTextAlign() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      valuetype AntdUI.TAlign AntdUI.Select::'<DropDownTextAlign>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_DropDownTextAlign

  .method public hidebysig specialname instance void 
          set_DropDownTextAlign(valuetype AntdUI.TAlign 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      valuetype AntdUI.TAlign AntdUI.Select::'<DropDownTextAlign>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_DropDownTextAlign

  .method public hidebysig specialname instance bool 
          get_EnterDropDown() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<EnterDropDown>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_EnterDropDown

  .method public hidebysig specialname instance void 
          set_EnterDropDown(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<EnterDropDown>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_EnterDropDown

  .method public hidebysig specialname instance bool 
          get_DropDownEmptyClose() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<DropDownEmptyClose>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_DropDownEmptyClose

  .method public hidebysig specialname instance void 
          set_DropDownEmptyClose(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<DropDownEmptyClose>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_DropDownEmptyClose

  .method public hidebysig specialname instance bool 
          get_ClickEnd() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<ClickEnd>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_ClickEnd

  .method public hidebysig specialname instance void 
          set_ClickEnd(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<ClickEnd>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_ClickEnd

  .method public hidebysig specialname instance bool 
          get_ClickSwitchDropdown() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<ClickSwitchDropdown>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_ClickSwitchDropdown

  .method public hidebysig specialname instance void 
          set_ClickSwitchDropdown(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<ClickSwitchDropdown>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_ClickSwitchDropdown

  .method public hidebysig specialname instance bool 
          get_CloseIcon() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<CloseIcon>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_CloseIcon

  .method public hidebysig specialname instance void 
          set_CloseIcon(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<CloseIcon>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_CloseIcon

  .method public hidebysig specialname instance bool 
          get_Empty() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<Empty>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_Empty

  .method public hidebysig specialname instance void 
          set_Empty(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<Empty>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_Empty

  .method public hidebysig specialname instance bool 
          get_AutoPrefixSvg() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<AutoPrefixSvg>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_AutoPrefixSvg

  .method public hidebysig specialname instance void 
          set_AutoPrefixSvg(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<AutoPrefixSvg>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_AutoPrefixSvg

  .method public hidebysig specialname instance bool 
          get_AutoText() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<AutoText>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_AutoText

  .method public hidebysig specialname instance void 
          set_AutoText(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<AutoText>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_AutoText

  .method public hidebysig specialname instance bool 
          get_WheelModifyEnabled() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::'<WheelModifyEnabled>k__BackingField'
    IL_0006:  ret
  } // end of method Select::get_WheelModifyEnabled

  .method public hidebysig specialname instance void 
          set_WheelModifyEnabled(bool 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      bool AntdUI.Select::'<WheelModifyEnabled>k__BackingField'
    IL_0007:  ret
  } // end of method Select::set_WheelModifyEnabled

  .method public hidebysig specialname instance class AntdUI.BaseCollection 
          get_Items() cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       26 (0x1a)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0006:  brtrue.s   IL_0013

    IL_0008:  ldarg.0
    IL_0009:  newobj     instance void AntdUI.BaseCollection::.ctor()
    IL_000e:  stfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0013:  ldarg.0
    IL_0014:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0019:  ret
  } // end of method Select::get_Items

  .method public hidebysig specialname instance void 
          set_Items(class AntdUI.BaseCollection 'value') cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0007:  ret
  } // end of method Select::set_Items

  .method public hidebysig specialname instance int32 
          get_SelectedIndex() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_0006:  ret
  } // end of method Select::get_SelectedIndex

  .method public hidebysig specialname instance void 
          set_SelectedIndex(int32 'value') cil managed
  {
    // 代码大小       101 (0x65)
    .maxstack  3
    .locals init (object V_0)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldnull
    IL_000c:  stfld      string AntdUI.Select::filtertext
    IL_0011:  ldarg.0
    IL_0012:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0017:  brfalse.s  IL_002a

    IL_0019:  ldarg.0
    IL_001a:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_001f:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_0024:  brfalse.s  IL_002a

    IL_0026:  ldarg.1
    IL_0027:  ldc.i4.m1
    IL_0028:  bne.un.s   IL_0032

    IL_002a:  ldarg.0
    IL_002b:  call       instance void AntdUI.Select::ChangeValueNULL()
    IL_0030:  br.s       IL_004b

    IL_0032:  ldarg.0
    IL_0033:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0038:  ldarg.1
    IL_0039:  callvirt   instance object AntdUI.BaseCollection::get_Item(int32)
    IL_003e:  stloc.0
    IL_003f:  ldloc.0
    IL_0040:  brtrue.s   IL_0043

    IL_0042:  ret

    IL_0043:  ldarg.0
    IL_0044:  ldarg.1
    IL_0045:  ldloc.0
    IL_0046:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                  object)
    IL_004b:  ldarg.0
    IL_004c:  ldfld      bool AntdUI.Select::_list
    IL_0051:  brfalse.s  IL_0059

    IL_0053:  ldarg.0
    IL_0054:  callvirt   instance void AntdUI.Input::Invalidate()
    IL_0059:  ldarg.0
    IL_005a:  ldstr      "SelectedIndex"
    IL_005f:  call       instance void AntdUI.IControl::OnPropertyChanged(string)
    IL_0064:  ret
  } // end of method Select::set_SelectedIndex

  .method public hidebysig specialname instance object 
          get_SelectedValue() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      object AntdUI.Select::selectedValue
    IL_0006:  ret
  } // end of method Select::get_SelectedValue

  .method public hidebysig specialname instance void 
          set_SelectedValue(object 'value') cil managed
  {
    // 代码大小       88 (0x58)
    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  ldfld      object AntdUI.Select::selectedValue
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldnull
    IL_000c:  stfld      string AntdUI.Select::filtertext
    IL_0011:  ldarg.1
    IL_0012:  brfalse.s  IL_0029

    IL_0014:  ldarg.0
    IL_0015:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_001a:  brfalse.s  IL_0029

    IL_001c:  ldarg.0
    IL_001d:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0022:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_0027:  brtrue.s   IL_0031

    IL_0029:  ldarg.0
    IL_002a:  call       instance void AntdUI.Select::ChangeValueNULL()
    IL_002f:  br.s       IL_003e

    IL_0031:  ldarg.0
    IL_0032:  ldarg.0
    IL_0033:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0038:  ldarg.1
    IL_0039:  call       instance void AntdUI.Select::SetChangeValue(class AntdUI.BaseCollection,
                                                                     object)
    IL_003e:  ldarg.0
    IL_003f:  ldfld      bool AntdUI.Select::_list
    IL_0044:  brfalse.s  IL_004c

    IL_0046:  ldarg.0
    IL_0047:  callvirt   instance void AntdUI.Input::Invalidate()
    IL_004c:  ldarg.0
    IL_004d:  ldstr      "SelectedValue"
    IL_0052:  call       instance void AntdUI.IControl::OnPropertyChanged(string)
    IL_0057:  ret
  } // end of method Select::set_SelectedValue

  .method private hidebysig instance void 
          ChangeValueNULL() cil managed
  {
    // 代码大小       71 (0x47)
    .maxstack  2
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.1
    IL_0002:  stfld      bool AntdUI.Select::noex
    IL_0007:  ldarg.0
    IL_0008:  ldstr      ""
    IL_000d:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_0012:  ldarg.0
    IL_0013:  ldnull
    IL_0014:  stfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_0019:  ldarg.0
    IL_001a:  ldnull
    IL_001b:  stfld      object AntdUI.Select::selectedValue
    IL_0020:  ldarg.0
    IL_0021:  ldc.i4.m1
    IL_0022:  stfld      int32 AntdUI.Select::selectedIndex
    IL_0027:  ldarg.0
    IL_0028:  ldarg.0
    IL_0029:  ldfld      object AntdUI.Select::selectedValue
    IL_002e:  callvirt   instance void AntdUI.Select::OnSelectedValueChanged(object)
    IL_0033:  ldarg.0
    IL_0034:  ldarg.0
    IL_0035:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_003a:  callvirt   instance void AntdUI.Select::OnSelectedIndexChanged(int32)
    IL_003f:  ldarg.0
    IL_0040:  ldc.i4.0
    IL_0041:  stfld      bool AntdUI.Select::noex
    IL_0046:  ret
  } // end of method Select::ChangeValueNULL

  .method private hidebysig instance void 
          ChangeValue(int32 'value',
                      object obj) cil managed
  {
    // 代码大小       162 (0xa2)
    .maxstack  3
    .locals init (class AntdUI.SelectItem V_0)
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      int32 AntdUI.Select::selectedIndex
    IL_0007:  ldarg.2
    IL_0008:  isinst     AntdUI.SelectItem
    IL_000d:  stloc.0
    IL_000e:  ldloc.0
    IL_000f:  brfalse.s  IL_004e

    IL_0011:  ldarg.0
    IL_0012:  ldloc.0
    IL_0013:  stfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_0018:  ldarg.0
    IL_0019:  ldloc.0
    IL_001a:  callvirt   instance object AntdUI.SelectItem::get_Tag()
    IL_001f:  stfld      object AntdUI.Select::selectedValue
    IL_0024:  ldarg.0
    IL_0025:  call       instance bool AntdUI.Select::get_AutoPrefixSvg()
    IL_002a:  brfalse.s  IL_0038

    IL_002c:  ldarg.0
    IL_002d:  ldloc.0
    IL_002e:  callvirt   instance string AntdUI.SelectItem::get_IconSvg()
    IL_0033:  call       instance void AntdUI.Input::set_PrefixSvg(string)
    IL_0038:  ldarg.0
    IL_0039:  call       instance bool AntdUI.Select::get_AutoText()
    IL_003e:  brfalse.s  IL_0089

    IL_0040:  ldarg.0
    IL_0041:  ldloc.0
    IL_0042:  callvirt   instance string AntdUI.SelectItem::get_Text()
    IL_0047:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_004c:  br.s       IL_0089

    IL_004e:  ldarg.0
    IL_004f:  ldnull
    IL_0050:  stfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_0055:  ldarg.0
    IL_0056:  ldarg.2
    IL_0057:  stfld      object AntdUI.Select::selectedValue
    IL_005c:  ldarg.0
    IL_005d:  call       instance bool AntdUI.Select::get_AutoText()
    IL_0062:  brfalse.s  IL_0089

    IL_0064:  ldarg.2
    IL_0065:  brtrue.s   IL_0074

    IL_0067:  ldarg.0
    IL_0068:  ldstr      ""
    IL_006d:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_0072:  br.s       IL_0089

    IL_0074:  ldarg.0
    IL_0075:  ldarg.2
    IL_0076:  callvirt   instance string [mscorlib]System.Object::ToString()
    IL_007b:  dup
    IL_007c:  brtrue.s   IL_0084

    IL_007e:  pop
    IL_007f:  ldstr      ""
    IL_0084:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_0089:  ldarg.0
    IL_008a:  ldarg.0
    IL_008b:  ldfld      object AntdUI.Select::selectedValue
    IL_0090:  callvirt   instance void AntdUI.Select::OnSelectedValueChanged(object)
    IL_0095:  ldarg.0
    IL_0096:  ldarg.0
    IL_0097:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_009c:  callvirt   instance void AntdUI.Select::OnSelectedIndexChanged(int32)
    IL_00a1:  ret
  } // end of method Select::ChangeValue

  .method private hidebysig instance void 
          SetChangeValue(class AntdUI.BaseCollection items,
                         object val) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       398 (0x18e)
    .maxstack  3
    .locals init (int32 V_0,
             object V_1,
             class AntdUI.SelectItem V_2,
             class AntdUI.SelectItem V_3,
             class [mscorlib]System.Collections.Generic.IEnumerator`1<object> V_4,
             object V_5,
             class AntdUI.SelectItem V_6,
             class AntdUI.GroupSelectItem V_7,
             object V_8,
             class AntdUI.SelectItem V_9)
    IL_0000:  ldc.i4.0
    IL_0001:  stloc.0
    IL_0002:  br         IL_0173

    IL_0007:  ldarg.1
    IL_0008:  ldloc.0
    IL_0009:  callvirt   instance object AntdUI.BaseCollection::get_Item(int32)
    IL_000e:  stloc.1
    IL_000f:  ldarg.2
    IL_0010:  ldloc.1
    IL_0011:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
    IL_0016:  brfalse.s  IL_0021

    IL_0018:  ldarg.0
    IL_0019:  ldloc.0
    IL_001a:  ldloc.1
    IL_001b:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                  object)
    IL_0020:  ret

    IL_0021:  ldloc.1
    IL_0022:  isinst     AntdUI.SelectItem
    IL_0027:  stloc.2
    IL_0028:  ldloc.2
    IL_0029:  brfalse.s  IL_0042

    IL_002b:  ldloc.2
    IL_002c:  callvirt   instance object AntdUI.SelectItem::get_Tag()
    IL_0031:  ldarg.2
    IL_0032:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
    IL_0037:  brfalse.s  IL_0042

    IL_0039:  ldarg.0
    IL_003a:  ldloc.0
    IL_003b:  ldloc.1
    IL_003c:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                  object)
    IL_0041:  ret

    IL_0042:  ldloc.1
    IL_0043:  isinst     AntdUI.SelectItem
    IL_0048:  stloc.3
    IL_0049:  ldloc.3
    IL_004a:  brfalse    IL_00dc

    IL_004f:  ldloc.3
    IL_0050:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.SelectItem::get_Sub()
    IL_0055:  brfalse    IL_00dc

    IL_005a:  ldloc.3
    IL_005b:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.SelectItem::get_Sub()
    IL_0060:  callvirt   instance int32 class [mscorlib]System.Collections.Generic.ICollection`1<object>::get_Count()
    IL_0065:  ldc.i4.0
    IL_0066:  ble.s      IL_00dc

    IL_0068:  ldloc.3
    IL_0069:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.SelectItem::get_Sub()
    IL_006e:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<object>::GetEnumerator()
    IL_0073:  stloc.s    V_4
    .try
    {
      IL_0075:  br.s       IL_00c2

      IL_0077:  ldloc.s    V_4
      IL_0079:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<object>::get_Current()
      IL_007e:  stloc.s    V_5
      IL_0080:  ldarg.2
      IL_0081:  ldloc.s    V_5
      IL_0083:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
      IL_0088:  brfalse.s  IL_0098

      IL_008a:  ldarg.0
      IL_008b:  ldloc.0
      IL_008c:  ldloc.s    V_5
      IL_008e:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                    object)
      IL_0093:  leave      IL_018d

      IL_0098:  ldloc.s    V_5
      IL_009a:  isinst     AntdUI.SelectItem
      IL_009f:  stloc.s    V_6
      IL_00a1:  ldloc.s    V_6
      IL_00a3:  brfalse.s  IL_00c2

      IL_00a5:  ldloc.s    V_6
      IL_00a7:  callvirt   instance object AntdUI.SelectItem::get_Tag()
      IL_00ac:  ldarg.2
      IL_00ad:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
      IL_00b2:  brfalse.s  IL_00c2

      IL_00b4:  ldarg.0
      IL_00b5:  ldloc.0
      IL_00b6:  ldloc.s    V_5
      IL_00b8:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                    object)
      IL_00bd:  leave      IL_018d

      IL_00c2:  ldloc.s    V_4
      IL_00c4:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00c9:  brtrue.s   IL_0077

      IL_00cb:  leave      IL_016f

    }  // end .try
    finally
    {
      IL_00d0:  ldloc.s    V_4
      IL_00d2:  brfalse.s  IL_00db

      IL_00d4:  ldloc.s    V_4
      IL_00d6:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00db:  endfinally
    }  // end handler
    IL_00dc:  ldloc.1
    IL_00dd:  isinst     AntdUI.GroupSelectItem
    IL_00e2:  stloc.s    V_7
    IL_00e4:  ldloc.s    V_7
    IL_00e6:  brfalse    IL_016f

    IL_00eb:  ldloc.s    V_7
    IL_00ed:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.GroupSelectItem::get_Sub()
    IL_00f2:  brfalse.s  IL_016f

    IL_00f4:  ldloc.s    V_7
    IL_00f6:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.GroupSelectItem::get_Sub()
    IL_00fb:  callvirt   instance int32 class [mscorlib]System.Collections.Generic.ICollection`1<object>::get_Count()
    IL_0100:  ldc.i4.0
    IL_0101:  ble.s      IL_016f

    IL_0103:  ldloc.s    V_7
    IL_0105:  callvirt   instance class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.GroupSelectItem::get_Sub()
    IL_010a:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<object>::GetEnumerator()
    IL_010f:  stloc.s    V_4
    .try
    {
      IL_0111:  br.s       IL_0158

      IL_0113:  ldloc.s    V_4
      IL_0115:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<object>::get_Current()
      IL_011a:  stloc.s    V_8
      IL_011c:  ldarg.2
      IL_011d:  ldloc.s    V_8
      IL_011f:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
      IL_0124:  brfalse.s  IL_0131

      IL_0126:  ldarg.0
      IL_0127:  ldloc.0
      IL_0128:  ldloc.s    V_8
      IL_012a:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                    object)
      IL_012f:  leave.s    IL_018d

      IL_0131:  ldloc.s    V_8
      IL_0133:  isinst     AntdUI.SelectItem
      IL_0138:  stloc.s    V_9
      IL_013a:  ldloc.s    V_9
      IL_013c:  brfalse.s  IL_0158

      IL_013e:  ldloc.s    V_9
      IL_0140:  callvirt   instance object AntdUI.SelectItem::get_Tag()
      IL_0145:  ldarg.2
      IL_0146:  callvirt   instance bool [mscorlib]System.Object::Equals(object)
      IL_014b:  brfalse.s  IL_0158

      IL_014d:  ldarg.0
      IL_014e:  ldloc.0
      IL_014f:  ldloc.s    V_8
      IL_0151:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                    object)
      IL_0156:  leave.s    IL_018d

      IL_0158:  ldloc.s    V_4
      IL_015a:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_015f:  brtrue.s   IL_0113

      IL_0161:  leave.s    IL_016f

    }  // end .try
    finally
    {
      IL_0163:  ldloc.s    V_4
      IL_0165:  brfalse.s  IL_016e

      IL_0167:  ldloc.s    V_4
      IL_0169:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_016e:  endfinally
    }  // end handler
    IL_016f:  ldloc.0
    IL_0170:  ldc.i4.1
    IL_0171:  add
    IL_0172:  stloc.0
    IL_0173:  ldloc.0
    IL_0174:  ldarg.1
    IL_0175:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_017a:  blt        IL_0007

    IL_017f:  ldarg.0
    IL_0180:  ldarg.1
    IL_0181:  ldarg.2
    IL_0182:  callvirt   instance int32 AntdUI.BaseCollection::IndexOf(object)
    IL_0187:  ldarg.2
    IL_0188:  call       instance void AntdUI.Select::ChangeValue(int32,
                                                                  object)
    IL_018d:  ret
  } // end of method Select::SetChangeValue

  .method assembly hidebysig instance void 
          DropDownChange(int32 x,
                         int32 y,
                         object 'value',
                         class AntdUI.SelectItem item,
                         string text) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    .param [4]
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 02 00 00 ) 
    // 代码大小       173 (0xad)
    .maxstack  4
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      int32 AntdUI.Select::selectedIndexX
    IL_0007:  ldarg.0
    IL_0008:  ldarg.2
    IL_0009:  stfld      int32 AntdUI.Select::selectedIndex
    IL_000e:  ldarg.0
    IL_000f:  ldarg.s    item
    IL_0011:  stfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_0016:  ldarg.0
    IL_0017:  ldarg.3
    IL_0018:  stfld      object AntdUI.Select::selectedValue
    IL_001d:  ldarg.0
    IL_001e:  call       instance bool AntdUI.Select::get_AutoText()
    IL_0023:  brfalse.s  IL_002d

    IL_0025:  ldarg.0
    IL_0026:  ldarg.s    text
    IL_0028:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_002d:  ldarg.0
    IL_002e:  call       instance bool AntdUI.Select::get_AutoPrefixSvg()
    IL_0033:  brfalse.s  IL_0046

    IL_0035:  ldarg.s    item
    IL_0037:  brfalse.s  IL_0046

    IL_0039:  ldarg.0
    IL_003a:  ldarg.s    item
    IL_003c:  callvirt   instance string AntdUI.SelectItem::get_IconSvg()
    IL_0041:  call       instance void AntdUI.Input::set_PrefixSvg(string)
    IL_0046:  ldarg.0
    IL_0047:  ldarg.0
    IL_0048:  ldfld      object AntdUI.Select::selectedValue
    IL_004d:  callvirt   instance void AntdUI.Select::OnSelectedValueChanged(object)
    IL_0052:  ldarg.0
    IL_0053:  ldarg.0
    IL_0054:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_0059:  callvirt   instance void AntdUI.Select::OnSelectedIndexChanged(int32)
    IL_005e:  ldarg.0
    IL_005f:  ldfld      class AntdUI.IntXYEventHandler AntdUI.Select::SelectedIndexsChanged
    IL_0064:  dup
    IL_0065:  brtrue.s   IL_006a

    IL_0067:  pop
    IL_0068:  br.s       IL_0081

    IL_006a:  ldarg.0
    IL_006b:  ldarg.0
    IL_006c:  ldfld      int32 AntdUI.Select::selectedIndexX
    IL_0071:  ldarg.0
    IL_0072:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_0077:  newobj     instance void AntdUI.IntXYEventArgs::.ctor(int32,
                                                                    int32)
    IL_007c:  callvirt   instance void AntdUI.IntXYEventHandler::Invoke(object,
                                                                        class AntdUI.IntXYEventArgs)
    IL_0081:  ldarg.0
    IL_0082:  ldstr      "SelectedIndex"
    IL_0087:  call       instance void AntdUI.IControl::OnPropertyChanged(string)
    IL_008c:  ldarg.0
    IL_008d:  ldstr      "SelectedValue"
    IL_0092:  call       instance void AntdUI.IControl::OnPropertyChanged(string)
    IL_0097:  ldarg.0
    IL_0098:  ldc.i4.0
    IL_0099:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_009e:  ldarg.0
    IL_009f:  ldc.i4.0
    IL_00a0:  stfld      int32 AntdUI.Select::select_x
    IL_00a5:  ldarg.0
    IL_00a6:  ldnull
    IL_00a7:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_00ac:  ret
  } // end of method Select::DropDownChange

  .method assembly hidebysig instance bool 
          DropDownClose(object 'value') cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       30 (0x1e)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_0006:  brtrue.s   IL_000a

    IL_0008:  ldc.i4.0
    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_0010:  ldarg.0
    IL_0011:  ldarg.1
    IL_0012:  newobj     instance void AntdUI.ObjectNEventArgs::.ctor(object)
    IL_0017:  callvirt   instance void AntdUI.ObjectNEventHandler::Invoke(object,
                                                                          class AntdUI.ObjectNEventArgs)
    IL_001c:  ldc.i4.1
    IL_001d:  ret
  } // end of method Select::DropDownClose

  .method public hidebysig specialname instance void 
          add_SelectedIndexChanged(class AntdUI.IntEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.IntEventHandler V_0,
             class AntdUI.IntEventHandler V_1,
             class AntdUI.IntEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.IntEventHandler AntdUI.Select::SelectedIndexChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.IntEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.IntEventHandler AntdUI.Select::SelectedIndexChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.IntEventHandler>(!!0&,
                                                                                                                   !!0,
                                                                                                                   !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_SelectedIndexChanged

  .method public hidebysig specialname instance void 
          remove_SelectedIndexChanged(class AntdUI.IntEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.IntEventHandler V_0,
             class AntdUI.IntEventHandler V_1,
             class AntdUI.IntEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.IntEventHandler AntdUI.Select::SelectedIndexChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.IntEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.IntEventHandler AntdUI.Select::SelectedIndexChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.IntEventHandler>(!!0&,
                                                                                                                   !!0,
                                                                                                                   !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_SelectedIndexChanged

  .method family hidebysig newslot virtual 
          instance void  OnSelectedIndexChanged(int32 e) cil managed
  {
    // 代码大小       24 (0x18)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.IntEventHandler AntdUI.Select::SelectedIndexChanged
    IL_0006:  dup
    IL_0007:  brtrue.s   IL_000b

    IL_0009:  pop
    IL_000a:  ret

    IL_000b:  ldarg.0
    IL_000c:  ldarg.1
    IL_000d:  newobj     instance void AntdUI.IntEventArgs::.ctor(int32)
    IL_0012:  callvirt   instance void AntdUI.IntEventHandler::Invoke(object,
                                                                      class AntdUI.IntEventArgs)
    IL_0017:  ret
  } // end of method Select::OnSelectedIndexChanged

  .method public hidebysig specialname instance void 
          add_SelectedIndexsChanged(class AntdUI.IntXYEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.IntXYEventHandler V_0,
             class AntdUI.IntXYEventHandler V_1,
             class AntdUI.IntXYEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.IntXYEventHandler AntdUI.Select::SelectedIndexsChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.IntXYEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.IntXYEventHandler AntdUI.Select::SelectedIndexsChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.IntXYEventHandler>(!!0&,
                                                                                                                     !!0,
                                                                                                                     !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_SelectedIndexsChanged

  .method public hidebysig specialname instance void 
          remove_SelectedIndexsChanged(class AntdUI.IntXYEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.IntXYEventHandler V_0,
             class AntdUI.IntXYEventHandler V_1,
             class AntdUI.IntXYEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.IntXYEventHandler AntdUI.Select::SelectedIndexsChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.IntXYEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.IntXYEventHandler AntdUI.Select::SelectedIndexsChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.IntXYEventHandler>(!!0&,
                                                                                                                     !!0,
                                                                                                                     !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_SelectedIndexsChanged

  .method public hidebysig specialname instance void 
          add_SelectedValueChanged(class AntdUI.ObjectNEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.ObjectNEventHandler V_0,
             class AntdUI.ObjectNEventHandler V_1,
             class AntdUI.ObjectNEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::SelectedValueChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.ObjectNEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.ObjectNEventHandler AntdUI.Select::SelectedValueChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.ObjectNEventHandler>(!!0&,
                                                                                                                       !!0,
                                                                                                                       !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_SelectedValueChanged

  .method public hidebysig specialname instance void 
          remove_SelectedValueChanged(class AntdUI.ObjectNEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.ObjectNEventHandler V_0,
             class AntdUI.ObjectNEventHandler V_1,
             class AntdUI.ObjectNEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::SelectedValueChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.ObjectNEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.ObjectNEventHandler AntdUI.Select::SelectedValueChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.ObjectNEventHandler>(!!0&,
                                                                                                                       !!0,
                                                                                                                       !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_SelectedValueChanged

  .method family hidebysig newslot virtual 
          instance void  OnSelectedValueChanged(object e) cil managed
  {
    // 代码大小       24 (0x18)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::SelectedValueChanged
    IL_0006:  dup
    IL_0007:  brtrue.s   IL_000b

    IL_0009:  pop
    IL_000a:  ret

    IL_000b:  ldarg.0
    IL_000c:  ldarg.1
    IL_000d:  newobj     instance void AntdUI.ObjectNEventArgs::.ctor(object)
    IL_0012:  callvirt   instance void AntdUI.ObjectNEventHandler::Invoke(object,
                                                                          class AntdUI.ObjectNEventArgs)
    IL_0017:  ret
  } // end of method Select::OnSelectedValueChanged

  .method public hidebysig specialname instance void 
          add_ExpandDropChanged(class AntdUI.BoolEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.BoolEventHandler V_0,
             class AntdUI.BoolEventHandler V_1,
             class AntdUI.BoolEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.BoolEventHandler AntdUI.Select::ExpandDropChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.BoolEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.BoolEventHandler AntdUI.Select::ExpandDropChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.BoolEventHandler>(!!0&,
                                                                                                                    !!0,
                                                                                                                    !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_ExpandDropChanged

  .method public hidebysig specialname instance void 
          remove_ExpandDropChanged(class AntdUI.BoolEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.BoolEventHandler V_0,
             class AntdUI.BoolEventHandler V_1,
             class AntdUI.BoolEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.BoolEventHandler AntdUI.Select::ExpandDropChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.BoolEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.BoolEventHandler AntdUI.Select::ExpandDropChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.BoolEventHandler>(!!0&,
                                                                                                                    !!0,
                                                                                                                    !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_ExpandDropChanged

  .method family hidebysig newslot virtual 
          instance void  OnExpandDropChanged(bool e) cil managed
  {
    // 代码大小       24 (0x18)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.BoolEventHandler AntdUI.Select::ExpandDropChanged
    IL_0006:  dup
    IL_0007:  brtrue.s   IL_000b

    IL_0009:  pop
    IL_000a:  ret

    IL_000b:  ldarg.0
    IL_000c:  ldarg.1
    IL_000d:  newobj     instance void AntdUI.BoolEventArgs::.ctor(bool)
    IL_0012:  callvirt   instance void AntdUI.BoolEventHandler::Invoke(object,
                                                                       class AntdUI.BoolEventArgs)
    IL_0017:  ret
  } // end of method Select::OnExpandDropChanged

  .method public hidebysig specialname instance void 
          add_ClosedItem(class AntdUI.ObjectNEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.ObjectNEventHandler V_0,
             class AntdUI.ObjectNEventHandler V_1,
             class AntdUI.ObjectNEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.ObjectNEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.ObjectNEventHandler>(!!0&,
                                                                                                                       !!0,
                                                                                                                       !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_ClosedItem

  .method public hidebysig specialname instance void 
          remove_ClosedItem(class AntdUI.ObjectNEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.ObjectNEventHandler V_0,
             class AntdUI.ObjectNEventHandler V_1,
             class AntdUI.ObjectNEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.ObjectNEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.ObjectNEventHandler AntdUI.Select::ClosedItem
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.ObjectNEventHandler>(!!0&,
                                                                                                                       !!0,
                                                                                                                       !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_ClosedItem

  .method public hidebysig specialname instance void 
          add_FilterChanged(class AntdUI.Select/FilterEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.Select/FilterEventHandler V_0,
             class AntdUI.Select/FilterEventHandler V_1,
             class AntdUI.Select/FilterEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate,
                                                                                            class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.Select/FilterEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.Select/FilterEventHandler>(!!0&,
                                                                                                                             !!0,
                                                                                                                             !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::add_FilterChanged

  .method public hidebysig specialname instance void 
          remove_FilterChanged(class AntdUI.Select/FilterEventHandler 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       41 (0x29)
    .maxstack  3
    .locals init (class AntdUI.Select/FilterEventHandler V_0,
             class AntdUI.Select/FilterEventHandler V_1,
             class AntdUI.Select/FilterEventHandler V_2)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldarg.1
    IL_000b:  call       class [mscorlib]System.Delegate [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate,
                                                                                           class [mscorlib]System.Delegate)
    IL_0010:  castclass  AntdUI.Select/FilterEventHandler
    IL_0015:  stloc.2
    IL_0016:  ldarg.0
    IL_0017:  ldflda     class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
    IL_001c:  ldloc.2
    IL_001d:  ldloc.1
    IL_001e:  call       !!0 [mscorlib]System.Threading.Interlocked::CompareExchange<class AntdUI.Select/FilterEventHandler>(!!0&,
                                                                                                                             !!0,
                                                                                                                             !!0)
    IL_0023:  stloc.0
    IL_0024:  ldloc.0
    IL_0025:  ldloc.1
    IL_0026:  bne.un.s   IL_0007

    IL_0028:  ret
  } // end of method Select::remove_FilterChanged

  .method family hidebysig virtual instance void 
          OnTextChanged(class [mscorlib]System.EventArgs e) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       147 (0x93)
    .maxstack  2
    .locals init (class AntdUI.Select/'<>c__DisplayClass113_0' V_0)
    IL_0000:  newobj     instance void AntdUI.Select/'<>c__DisplayClass113_0'::.ctor()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  ldarg.0
    IL_0008:  stfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass113_0'::'<>4__this'
    IL_000d:  ldarg.0
    IL_000e:  ldarg.1
    IL_000f:  call       instance void [System.Windows.Forms]System.Windows.Forms.Control::OnTextChanged(class [mscorlib]System.EventArgs)
    IL_0014:  ldarg.0
    IL_0015:  call       instance bool AntdUI.Input::get_HasFocus()
    IL_001a:  brfalse.s  IL_0092

    IL_001c:  ldarg.0
    IL_001d:  ldfld      bool AntdUI.Select::noex
    IL_0022:  brtrue.s   IL_0037

    IL_0024:  ldarg.0
    IL_0025:  ldfld      string AntdUI.Select::filtertext
    IL_002a:  ldarg.0
    IL_002b:  callvirt   instance string [System.Windows.Forms]System.Windows.Forms.Control::get_Text()
    IL_0030:  call       bool [mscorlib]System.String::op_Equality(string,
                                                                   string)
    IL_0035:  brfalse.s  IL_0038

    IL_0037:  ret

    IL_0038:  ldarg.0
    IL_0039:  ldarg.0
    IL_003a:  callvirt   instance string [System.Windows.Forms]System.Windows.Forms.Control::get_Text()
    IL_003f:  stfld      string AntdUI.Select::filtertext
    IL_0044:  ldarg.0
    IL_0045:  ldfld      class AntdUI.Select/FilterEventHandler AntdUI.Select::FilterChanged
    IL_004a:  brtrue.s   IL_0073

    IL_004c:  ldarg.0
    IL_004d:  ldfld      bool AntdUI.Select::expandDrop
    IL_0052:  brfalse.s  IL_006b

    IL_0054:  ldarg.0
    IL_0055:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_005a:  dup
    IL_005b:  brtrue.s   IL_005f

    IL_005d:  pop
    IL_005e:  ret

    IL_005f:  ldarg.0
    IL_0060:  callvirt   instance string [System.Windows.Forms]System.Windows.Forms.Control::get_Text()
    IL_0065:  call       instance void AntdUI.LayeredFormSelectDown::TextChange(string)
    IL_006a:  ret

    IL_006b:  ldarg.0
    IL_006c:  ldc.i4.1
    IL_006d:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_0072:  ret

    IL_0073:  ldloc.0
    IL_0074:  ldarg.0
    IL_0075:  ldfld      string AntdUI.Select::filtertext
    IL_007a:  stfld      string AntdUI.Select/'<>c__DisplayClass113_0'::temp
    IL_007f:  ldloc.0
    IL_0080:  ldftn      instance void AntdUI.Select/'<>c__DisplayClass113_0'::'<OnTextChanged>b__0'()
    IL_0086:  newobj     instance void [mscorlib]System.Action::.ctor(object,
                                                                      native int)
    IL_008b:  ldnull
    IL_008c:  call       class [mscorlib]System.Threading.Tasks.Task AntdUI.ITask::Run(class [mscorlib]System.Action,
                                                                                       class [mscorlib]System.Action)
    IL_0091:  pop
    IL_0092:  ret
  } // end of method Select::OnTextChanged

  .method public hidebysig specialname instance bool 
          get_ShowIcon() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::showicon
    IL_0006:  ret
  } // end of method Select::get_ShowIcon

  .method public hidebysig specialname instance void 
          set_ShowIcon(bool 'value') cil managed
  {
    // 代码大小       31 (0x1f)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::showicon
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldarg.1
    IL_000c:  stfld      bool AntdUI.Select::showicon
    IL_0011:  ldarg.0
    IL_0012:  call       instance bool AntdUI.Input::CalculateRect()
    IL_0017:  pop
    IL_0018:  ldarg.0
    IL_0019:  callvirt   instance void AntdUI.Input::Invalidate()
    IL_001e:  ret
  } // end of method Select::set_ShowIcon

  .method public hidebysig specialname virtual 
          instance bool  get_HasSuffix() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::showicon
    IL_0006:  ret
  } // end of method Select::get_HasSuffix

  .method family hidebysig virtual instance void 
          PaintRIcon(class AntdUI.Canvas g,
                     valuetype [System.Drawing]System.Drawing.Rectangle rect_r) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       90 (0x5a)
    .maxstack  5
    .locals init (class [System.Drawing]System.Drawing.Pen V_0,
             valuetype [System.Drawing]System.Drawing.Drawing2D.LineCap V_1)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::showicon
    IL_0006:  brfalse.s  IL_0059

    IL_0008:  ldc.i4.s   41
    IL_000a:  ldstr      "Select"
    IL_000f:  ldarg.0
    IL_0010:  call       instance valuetype AntdUI.TAMode AntdUI.IControl::get_ColorScheme()
    IL_0015:  call       valuetype [System.Drawing]System.Drawing.Color AntdUI.Style::Get(valuetype AntdUI.Colour,
                                                                                          string,
                                                                                          valuetype AntdUI.TAMode)
    IL_001a:  ldc.r4     2.
    IL_001f:  newobj     instance void [System.Drawing]System.Drawing.Pen::.ctor(valuetype [System.Drawing]System.Drawing.Color,
                                                                                 float32)
    IL_0024:  stloc.0
    .try
    {
      IL_0025:  ldloc.0
      IL_0026:  ldloc.0
      IL_0027:  ldc.i4.2
      IL_0028:  dup
      IL_0029:  stloc.1
      IL_002a:  callvirt   instance void [System.Drawing]System.Drawing.Pen::set_EndCap(valuetype [System.Drawing]System.Drawing.Drawing2D.LineCap)
      IL_002f:  ldloc.1
      IL_0030:  callvirt   instance void [System.Drawing]System.Drawing.Pen::set_StartCap(valuetype [System.Drawing]System.Drawing.Drawing2D.LineCap)
      IL_0035:  ldarg.1
      IL_0036:  ldloc.0
      IL_0037:  ldarg.2
      IL_0038:  ldarg.0
      IL_0039:  ldfld      float32 AntdUI.Select::ArrowProg
      IL_003e:  ldc.r4     0.69999999
      IL_0043:  call       valuetype [System.Drawing]System.Drawing.PointF[] AntdUI.Helper::TriangleLinesVertical(valuetype [System.Drawing]System.Drawing.Rectangle,
                                                                                                                  float32,
                                                                                                                  float32)
      IL_0048:  callvirt   instance void AntdUI.Canvas::DrawLines(class [System.Drawing]System.Drawing.Pen,
                                                                  valuetype [System.Drawing]System.Drawing.PointF[])
      IL_004d:  leave.s    IL_0059

    }  // end .try
    finally
    {
      IL_004f:  ldloc.0
      IL_0050:  brfalse.s  IL_0058

      IL_0052:  ldloc.0
      IL_0053:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_0058:  endfinally
    }  // end handler
    IL_0059:  ret
  } // end of method Select::PaintRIcon

  .method public hidebysig newslot virtual final 
          instance class AntdUI.ILayeredForm 
          SubForm() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_0006:  ret
  } // end of method Select::SubForm

  .method public hidebysig specialname instance bool 
          get_Expand() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::expand
    IL_0006:  ret
  } // end of method Select::get_Expand

  .method public hidebysig specialname instance void 
          set_Expand(bool 'value') cil managed
  {
    // 代码大小       116 (0x74)
    .maxstack  6
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::expand
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldarg.1
    IL_000c:  stfld      bool AntdUI.Select::expand
    IL_0011:  ldstr      "Select"
    IL_0016:  call       bool AntdUI.Config::HasAnimation(string)
    IL_001b:  brfalse.s  IL_005e

    IL_001d:  ldarg.0
    IL_001e:  ldfld      class AntdUI.AnimationTask AntdUI.Select::ThreadExpand
    IL_0023:  dup
    IL_0024:  brtrue.s   IL_0029

    IL_0026:  pop
    IL_0027:  br.s       IL_002e

    IL_0029:  call       instance void AntdUI.AnimationTask::Dispose()
    IL_002e:  ldarg.0
    IL_002f:  ldarg.0
    IL_0030:  ldftn      instance void AntdUI.Select::'<set_Expand>b__128_0'(float32)
    IL_0036:  newobj     instance void class [mscorlib]System.Action`1<float32>::.ctor(object,
                                                                                       native int)
    IL_003b:  ldc.i4.s   10
    IL_003d:  ldc.i4.s   10
    IL_003f:  ldc.i4.s   100
    IL_0041:  call       int32 AntdUI.Animation::TotalFrames(int32,
                                                             int32)
    IL_0046:  ldarg.1
    IL_0047:  ldc.i4.2
    IL_0048:  newobj     instance void AntdUI.AnimationFixedConfig::.ctor(class [mscorlib]System.Action`1<float32>,
                                                                          int32,
                                                                          int32,
                                                                          bool,
                                                                          valuetype AntdUI.AnimationType)
    IL_004d:  ldc.i4.1
    IL_004e:  call       instance class AntdUI.AnimationFixedConfig AntdUI.AnimationFixedConfig::SetArrow(bool)
    IL_0053:  newobj     instance void AntdUI.AnimationTask::.ctor(class AntdUI.IAnimationConfig)
    IL_0058:  stfld      class AntdUI.AnimationTask AntdUI.Select::ThreadExpand
    IL_005d:  ret

    IL_005e:  ldarg.0
    IL_005f:  ldarg.1
    IL_0060:  brtrue.s   IL_0069

    IL_0062:  ldc.r4     -1.
    IL_0067:  br.s       IL_006e

    IL_0069:  ldc.r4     1.
    IL_006e:  stfld      float32 AntdUI.Select::ArrowProg
    IL_0073:  ret
  } // end of method Select::set_Expand

  .method public hidebysig specialname instance bool 
          get_ExpandDrop() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::expandDrop
    IL_0006:  ret
  } // end of method Select::get_ExpandDrop

  .method public hidebysig specialname instance void 
          set_ExpandDrop(bool 'value') cil managed
  {
    // 代码大小       252 (0xfc)
    .maxstack  2
    .locals init (class [mscorlib]System.Collections.Generic.List`1<object> V_0,
             class [mscorlib]System.Collections.IEnumerator V_1,
             object V_2,
             class [mscorlib]System.IDisposable V_3)
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::expandDrop
    IL_0006:  ldarg.1
    IL_0007:  bne.un.s   IL_000a

    IL_0009:  ret

    IL_000a:  ldarg.0
    IL_000b:  ldarg.1
    IL_000c:  stfld      bool AntdUI.Select::expandDrop
    IL_0011:  ldarg.1
    IL_0012:  brfalse    IL_00d0

    IL_0017:  ldarg.0
    IL_0018:  call       instance bool AntdUI.Input::get_ReadOnly()
    IL_001d:  brtrue.s   IL_0034

    IL_001f:  ldarg.0
    IL_0020:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0025:  brfalse.s  IL_0034

    IL_0027:  ldarg.0
    IL_0028:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_002d:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_0032:  brtrue.s   IL_0077

    IL_0034:  ldarg.0
    IL_0035:  call       instance bool AntdUI.Select::get_Empty()
    IL_003a:  brfalse.s  IL_0055

    IL_003c:  ldarg.0
    IL_003d:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_0042:  brtrue.s   IL_0055

    IL_0044:  ldarg.0
    IL_0045:  ldc.i4.0
    IL_0046:  newobj     instance void class [mscorlib]System.Collections.Generic.List`1<object>::.ctor(int32)
    IL_004b:  call       instance void AntdUI.Select::ShowLayeredForm(class [mscorlib]System.Collections.Generic.IList`1<object>)
    IL_0050:  br         IL_00f4

    IL_0055:  ldarg.0
    IL_0056:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_005b:  dup
    IL_005c:  brtrue.s   IL_0061

    IL_005e:  pop
    IL_005f:  br.s       IL_0067

    IL_0061:  ldc.i4.0
    IL_0062:  call       instance void AntdUI.ILayeredForm::IClose(bool)
    IL_0067:  ldarg.0
    IL_0068:  ldnull
    IL_0069:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_006e:  ldarg.0
    IL_006f:  ldc.i4.0
    IL_0070:  stfld      bool AntdUI.Select::expandDrop
    IL_0075:  br.s       IL_00f4

    IL_0077:  ldarg.0
    IL_0078:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_007d:  brtrue.s   IL_00f4

    IL_007f:  ldarg.0
    IL_0080:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0085:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_008a:  newobj     instance void class [mscorlib]System.Collections.Generic.List`1<object>::.ctor(int32)
    IL_008f:  stloc.0
    IL_0090:  ldarg.0
    IL_0091:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0096:  callvirt   instance class [mscorlib]System.Collections.IEnumerator AntdUI.BaseCollection::GetEnumerator()
    IL_009b:  stloc.1
    .try
    {
      IL_009c:  br.s       IL_00ac

      IL_009e:  ldloc.1
      IL_009f:  callvirt   instance object [mscorlib]System.Collections.IEnumerator::get_Current()
      IL_00a4:  stloc.2
      IL_00a5:  ldloc.0
      IL_00a6:  ldloc.2
      IL_00a7:  callvirt   instance void class [mscorlib]System.Collections.Generic.List`1<object>::Add(!0)
      IL_00ac:  ldloc.1
      IL_00ad:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
      IL_00b2:  brtrue.s   IL_009e

      IL_00b4:  leave.s    IL_00c7

    }  // end .try
    finally
    {
      IL_00b6:  ldloc.1
      IL_00b7:  isinst     [mscorlib]System.IDisposable
      IL_00bc:  stloc.3
      IL_00bd:  ldloc.3
      IL_00be:  brfalse.s  IL_00c6

      IL_00c0:  ldloc.3
      IL_00c1:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
      IL_00c6:  endfinally
    }  // end handler
    IL_00c7:  ldarg.0
    IL_00c8:  ldloc.0
    IL_00c9:  call       instance void AntdUI.Select::ShowLayeredForm(class [mscorlib]System.Collections.Generic.IList`1<object>)
    IL_00ce:  br.s       IL_00f4

    IL_00d0:  ldarg.0
    IL_00d1:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_00d6:  dup
    IL_00d7:  brtrue.s   IL_00dc

    IL_00d9:  pop
    IL_00da:  br.s       IL_00e2

    IL_00dc:  ldc.i4.0
    IL_00dd:  call       instance void AntdUI.ILayeredForm::IClose(bool)
    IL_00e2:  ldarg.0
    IL_00e3:  ldnull
    IL_00e4:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
    IL_00e9:  ldarg.0
    IL_00ea:  ldstr      ""
    IL_00ef:  stfld      string AntdUI.Select::filtertext
    IL_00f4:  ldarg.0
    IL_00f5:  ldarg.1
    IL_00f6:  callvirt   instance void AntdUI.Select::OnExpandDropChanged(bool)
    IL_00fb:  ret
  } // end of method Select::set_ExpandDrop

  .method private hidebysig instance void 
          ShowLayeredForm(class [mscorlib]System.Collections.Generic.IList`1<object> list) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       128 (0x80)
    .maxstack  4
    .locals init (class AntdUI.Select/'<>c__DisplayClass134_0' V_0)
    IL_0000:  newobj     instance void AntdUI.Select/'<>c__DisplayClass134_0'::.ctor()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  ldarg.0
    IL_0008:  stfld      class AntdUI.Select AntdUI.Select/'<>c__DisplayClass134_0'::'<>4__this'
    IL_000d:  ldloc.0
    IL_000e:  ldarg.1
    IL_000f:  stfld      class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.Select/'<>c__DisplayClass134_0'::list
    .try
    {
      IL_0014:  ldarg.0
      IL_0015:  call       instance bool [System.Windows.Forms]System.Windows.Forms.Control::get_InvokeRequired()
      IL_001a:  brfalse.s  IL_0031

      IL_001c:  ldarg.0
      IL_001d:  ldloc.0
      IL_001e:  ldftn      instance void AntdUI.Select/'<>c__DisplayClass134_0'::'<ShowLayeredForm>b__0'()
      IL_0024:  newobj     instance void [mscorlib]System.Action::.ctor(object,
                                                                        native int)
      IL_0029:  call       instance class [mscorlib]System.IAsyncResult AntdUI.IControl::BeginInvoke(class [mscorlib]System.Action)
      IL_002e:  pop
      IL_002f:  leave.s    IL_007f

      IL_0031:  ldarg.0
      IL_0032:  ldc.i4.1
      IL_0033:  call       instance void AntdUI.Select::set_Expand(bool)
      IL_0038:  ldarg.0
      IL_0039:  ldarg.0
      IL_003a:  ldloc.0
      IL_003b:  ldfld      class [mscorlib]System.Collections.Generic.IList`1<object> AntdUI.Select/'<>c__DisplayClass134_0'::list
      IL_0040:  ldarg.0
      IL_0041:  ldfld      string AntdUI.Select::filtertext
      IL_0046:  newobj     instance void AntdUI.LayeredFormSelectDown::.ctor(class AntdUI.Select,
                                                                             class [mscorlib]System.Collections.Generic.IList`1<object>,
                                                                             string)
      IL_004b:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_0050:  ldarg.0
      IL_0051:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_0056:  ldarg.0
      IL_0057:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Form::Show(class [System.Windows.Forms]System.Windows.Forms.IWin32Window)
      IL_005c:  ldarg.0
      IL_005d:  ldfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_0062:  ldloc.0
      IL_0063:  ldftn      instance void AntdUI.Select/'<>c__DisplayClass134_0'::'<ShowLayeredForm>b__1'(object,
                                                                                                         class [mscorlib]System.EventArgs)
      IL_0069:  newobj     instance void [mscorlib]System.EventHandler::.ctor(object,
                                                                              native int)
      IL_006e:  callvirt   instance void [System]System.ComponentModel.Component::add_Disposed(class [mscorlib]System.EventHandler)
      IL_0073:  leave.s    IL_007f

    }  // end .try
    catch [mscorlib]System.Object 
    {
      IL_0075:  pop
      IL_0076:  ldarg.0
      IL_0077:  ldnull
      IL_0078:  stfld      class AntdUI.LayeredFormSelectDown AntdUI.Select::subForm
      IL_007d:  leave.s    IL_007f

    }  // end handler
    IL_007f:  ret
  } // end of method Select::ShowLayeredForm

  .method family hidebysig virtual instance bool 
          ProcessCmdKey(valuetype [System.Windows.Forms]System.Windows.Forms.Message& msg,
                        valuetype [System.Windows.Forms]System.Windows.Forms.Keys keyData) cil managed
  {
    // 代码大小       45 (0x2d)
    .maxstack  3
    .locals init (bool V_0)
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  ldarg.2
    IL_0003:  call       instance bool [System.Windows.Forms]System.Windows.Forms.Control::ProcessCmdKey(valuetype [System.Windows.Forms]System.Windows.Forms.Message&,
                                                                                                         valuetype [System.Windows.Forms]System.Windows.Forms.Keys)
    IL_0008:  stloc.0
    IL_0009:  ldarg.2
    IL_000a:  ldc.i4.s   13
    IL_000c:  beq.s      IL_001c

    IL_000e:  ldarg.2
    IL_000f:  ldc.i4.s   40
    IL_0011:  bne.un.s   IL_002b

    IL_0013:  ldarg.0
    IL_0014:  ldc.i4.1
    IL_0015:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_001a:  ldc.i4.1
    IL_001b:  ret

    IL_001c:  ldarg.0
    IL_001d:  call       instance bool AntdUI.Select::get_EnterDropDown()
    IL_0022:  brfalse.s  IL_002b

    IL_0024:  ldarg.0
    IL_0025:  ldc.i4.1
    IL_0026:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_002b:  ldloc.0
    IL_002c:  ret
  } // end of method Select::ProcessCmdKey

  .method family hidebysig virtual instance void 
          OnLostFocus(class [mscorlib]System.EventArgs e) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       15 (0xf)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.0
    IL_0002:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  call       instance void AntdUI.Input::OnLostFocus(class [mscorlib]System.EventArgs)
    IL_000e:  ret
  } // end of method Select::OnLostFocus

  .method family hidebysig virtual instance void 
          OnClearValue() cil managed
  {
    // 代码大小       45 (0x2d)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 AntdUI.Select::selectedIndex
    IL_0006:  ldc.i4.m1
    IL_0007:  bgt.s      IL_0019

    IL_0009:  ldarg.0
    IL_000a:  ldfld      object AntdUI.Select::selectedValue
    IL_000f:  brtrue.s   IL_0019

    IL_0011:  ldarg.0
    IL_0012:  ldfld      bool AntdUI.Input::isempty
    IL_0017:  brtrue.s   IL_002c

    IL_0019:  ldarg.0
    IL_001a:  ldnull
    IL_001b:  stfld      string AntdUI.Select::filtertext
    IL_0020:  ldarg.0
    IL_0021:  call       instance void AntdUI.Select::ChangeValueNULL()
    IL_0026:  ldarg.0
    IL_0027:  callvirt   instance void AntdUI.Input::Invalidate()
    IL_002c:  ret
  } // end of method Select::OnClearValue

  .method family hidebysig virtual instance void 
          OnClickContent() cil managed
  {
    // 代码大小       73 (0x49)
    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  ldfld      bool AntdUI.Select::_list
    IL_0006:  brtrue.s   IL_0010

    IL_0008:  ldarg.0
    IL_0009:  call       instance bool AntdUI.Select::get_ClickSwitchDropdown()
    IL_000e:  brfalse.s  IL_0020

    IL_0010:  ldarg.0
    IL_0011:  ldarg.0
    IL_0012:  ldfld      bool AntdUI.Select::expandDrop
    IL_0017:  ldc.i4.0
    IL_0018:  ceq
    IL_001a:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_001f:  ret

    IL_0020:  ldarg.0
    IL_0021:  call       instance bool AntdUI.Input::get_HasFocus()
    IL_0026:  brfalse.s  IL_0041

    IL_0028:  ldarg.0
    IL_0029:  ldfld      bool AntdUI.Select::expandDrop
    IL_002e:  brfalse.s  IL_0031

    IL_0030:  ret

    IL_0031:  ldarg.0
    IL_0032:  ldarg.0
    IL_0033:  ldfld      bool AntdUI.Select::expandDrop
    IL_0038:  ldc.i4.0
    IL_0039:  ceq
    IL_003b:  call       instance void AntdUI.Select::set_ExpandDrop(bool)
    IL_0040:  ret

    IL_0041:  ldarg.0
    IL_0042:  call       instance bool [System.Windows.Forms]System.Windows.Forms.Control::Focus()
    IL_0047:  pop
    IL_0048:  ret
  } // end of method Select::OnClickContent

  .method family hidebysig virtual instance void 
          OnMouseWheel(class [System.Windows.Forms]System.Windows.Forms.MouseEventArgs e) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       87 (0x57)
    .maxstack  4
    .locals init (class [System.Windows.Forms]System.Windows.Forms.HandledMouseEventArgs V_0)
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  call       instance void AntdUI.Input::OnMouseWheel(class [System.Windows.Forms]System.Windows.Forms.MouseEventArgs)
    IL_0007:  ldarg.0
    IL_0008:  call       instance bool AntdUI.Input::get_ReadOnly()
    IL_000d:  brtrue.s   IL_002c

    IL_000f:  ldarg.0
    IL_0010:  call       instance bool AntdUI.Select::get_WheelModifyEnabled()
    IL_0015:  brfalse.s  IL_002c

    IL_0017:  ldarg.0
    IL_0018:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_001d:  brfalse.s  IL_002c

    IL_001f:  ldarg.0
    IL_0020:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0025:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_002a:  brtrue.s   IL_002d

    IL_002c:  ret

    IL_002d:  ldarg.0
    IL_002e:  ldarg.0
    IL_002f:  ldarg.0
    IL_0030:  ldfld      class AntdUI.BaseCollection AntdUI.Select::items
    IL_0035:  ldarg.1
    IL_0036:  callvirt   instance int32 [System.Windows.Forms]System.Windows.Forms.MouseEventArgs::get_Delta()
    IL_003b:  call       instance int32 AntdUI.Select::IMouseWheel(class AntdUI.BaseCollection,
                                                                   int32)
    IL_0040:  call       instance void AntdUI.Select::set_SelectedIndex(int32)
    IL_0045:  ldarg.1
    IL_0046:  isinst     [System.Windows.Forms]System.Windows.Forms.HandledMouseEventArgs
    IL_004b:  stloc.0
    IL_004c:  ldloc.0
    IL_004d:  brfalse.s  IL_0056

    IL_004f:  ldloc.0
    IL_0050:  ldc.i4.1
    IL_0051:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.HandledMouseEventArgs::set_Handled(bool)
    IL_0056:  ret
  } // end of method Select::OnMouseWheel

  .method private hidebysig instance int32 
          IMouseWheel(class AntdUI.BaseCollection items,
                      int32 delta) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       151 (0x97)
    .maxstack  3
    .locals init (int32 V_0,
             int32 V_1,
             int32 V_2)
    IL_0000:  ldarg.1
    IL_0001:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_0006:  stloc.0
    IL_0007:  ldc.i4.0
    IL_0008:  stloc.1
    IL_0009:  ldarg.2
    IL_000a:  ldc.i4.0
    IL_000b:  ble.s      IL_0052

    IL_000d:  ldarg.0
    IL_000e:  call       instance int32 AntdUI.Select::get_SelectedIndex()
    IL_0013:  ldc.i4.0
    IL_0014:  ble.s      IL_0020

    IL_0016:  ldarg.0
    IL_0017:  call       instance int32 AntdUI.Select::get_SelectedIndex()
    IL_001c:  ldc.i4.1
    IL_001d:  sub
    IL_001e:  br.s       IL_0028

    IL_0020:  ldarg.1
    IL_0021:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_0026:  ldc.i4.1
    IL_0027:  sub
    IL_0028:  stloc.2
    IL_0029:  br.s       IL_0041

    IL_002b:  ldloc.1
    IL_002c:  ldc.i4.1
    IL_002d:  add
    IL_002e:  stloc.1
    IL_002f:  ldloc.1
    IL_0030:  ldloc.0
    IL_0031:  ble.s      IL_0035

    IL_0033:  ldc.i4.m1
    IL_0034:  ret

    IL_0035:  ldloc.2
    IL_0036:  ldc.i4.1
    IL_0037:  sub
    IL_0038:  stloc.2
    IL_0039:  ldloc.2
    IL_003a:  ldc.i4.0
    IL_003b:  bge.s      IL_0041

    IL_003d:  ldloc.0
    IL_003e:  ldc.i4.1
    IL_003f:  sub
    IL_0040:  stloc.2
    IL_0041:  ldarg.0
    IL_0042:  ldarg.1
    IL_0043:  ldloc.2
    IL_0044:  callvirt   instance object AntdUI.BaseCollection::get_Item(int32)
    IL_0049:  call       instance bool AntdUI.Select::WheelItem(object)
    IL_004e:  brtrue.s   IL_002b

    IL_0050:  br.s       IL_0095

    IL_0052:  ldarg.0
    IL_0053:  call       instance int32 AntdUI.Select::get_SelectedIndex()
    IL_0058:  ldarg.1
    IL_0059:  callvirt   instance int32 AntdUI.BaseCollection::get_Count()
    IL_005e:  ldc.i4.1
    IL_005f:  sub
    IL_0060:  bge.s      IL_006c

    IL_0062:  ldarg.0
    IL_0063:  call       instance int32 AntdUI.Select::get_SelectedIndex()
    IL_0068:  ldc.i4.1
    IL_0069:  add
    IL_006a:  br.s       IL_006d

    IL_006c:  ldc.i4.0
    IL_006d:  stloc.2
    IL_006e:  br.s       IL_0086

    IL_0070:  ldloc.1
    IL_0071:  ldc.i4.1
    IL_0072:  add
    IL_0073:  stloc.1
    IL_0074:  ldloc.1
    IL_0075:  ldloc.0
    IL_0076:  ble.s      IL_007a

    IL_0078:  ldc.i4.m1
    IL_0079:  ret

    IL_007a:  ldloc.2
    IL_007b:  ldc.i4.1
    IL_007c:  add
    IL_007d:  stloc.2
    IL_007e:  ldloc.2
    IL_007f:  ldloc.0
    IL_0080:  ldc.i4.1
    IL_0081:  sub
    IL_0082:  ble.s      IL_0086

    IL_0084:  ldc.i4.0
    IL_0085:  stloc.2
    IL_0086:  ldarg.0
    IL_0087:  ldarg.1
    IL_0088:  ldloc.2
    IL_0089:  callvirt   instance object AntdUI.BaseCollection::get_Item(int32)
    IL_008e:  call       instance bool AntdUI.Select::WheelItem(object)
    IL_0093:  brtrue.s   IL_0070

    IL_0095:  ldloc.2
    IL_0096:  ret
  } // end of method Select::IMouseWheel

  .method private hidebysig instance bool 
          WheelItem(object it) cil managed
  {
    // 代码大小       12 (0xc)
    .maxstack  8
    IL_0000:  ldarg.1
    IL_0001:  isinst     AntdUI.DividerSelectItem
    IL_0006:  brfalse.s  IL_000a

    IL_0008:  ldc.i4.1
    IL_0009:  ret

    IL_000a:  ldc.i4.0
    IL_000b:  ret
  } // end of method Select::WheelItem

  .method family hidebysig virtual instance void 
          OnHandleCreated(class [mscorlib]System.EventArgs e) cil managed
  {
    .custom instance void System.Runtime.CompilerServices.NullableContextAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    // 代码大小       14 (0xe)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       void AntdUI.EventHub::AddListener(class AntdUI.IEventListener)
    IL_0006:  ldarg.0
    IL_0007:  ldarg.1
    IL_0008:  call       instance void AntdUI.Input::OnHandleCreated(class [mscorlib]System.EventArgs)
    IL_000d:  ret
  } // end of method Select::OnHandleCreated

  .method public hidebysig newslot virtual final 
          instance void  HandleEvent(valuetype AntdUI.EventType id,
                                     object tag) cil managed
  {
    // 代码大小       39 (0x27)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_0006:  brtrue.s   IL_0009

    IL_0008:  ret

    IL_0009:  ldarg.1
    IL_000a:  ldc.i4.3
    IL_000b:  bne.un.s   IL_0026

    IL_000d:  ldarg.0
    IL_000e:  call       instance bool AntdUI.Select::get_AutoText()
    IL_0013:  brfalse.s  IL_0026

    IL_0015:  ldarg.0
    IL_0016:  ldarg.0
    IL_0017:  ldfld      class AntdUI.SelectItem AntdUI.Select::selectedItem
    IL_001c:  callvirt   instance string AntdUI.SelectItem::get_Text()
    IL_0021:  callvirt   instance void [System.Windows.Forms]System.Windows.Forms.Control::set_Text(string)
    IL_0026:  ret
  } // end of method Select::HandleEvent

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // 代码大小       104 (0x68)
    .maxstack  3
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.s   32
    IL_0003:  stfld      valuetype AntdUI.TAlignFrom AntdUI.Select::'<Placement>k__BackingField'
    IL_0008:  ldarg.0
    IL_0009:  ldc.i4.4
    IL_000a:  stfld      int32 AntdUI.Select::'<MaxCount>k__BackingField'
    IL_000f:  ldarg.0
    IL_0010:  ldc.i4.s   12
    IL_0012:  ldc.i4.5
    IL_0013:  newobj     instance void [System.Drawing]System.Drawing.Size::.ctor(int32,
                                                                                  int32)
    IL_0018:  stfld      valuetype [System.Drawing]System.Drawing.Size AntdUI.Select::'<DropDownPadding>k__BackingField'
    IL_001d:  ldarg.0
    IL_001e:  ldc.i4.s   40
    IL_0020:  stfld      valuetype AntdUI.TAlign AntdUI.Select::'<DropDownTextAlign>k__BackingField'
    IL_0025:  ldarg.0
    IL_0026:  ldc.i4.1
    IL_0027:  stfld      bool AntdUI.Select::'<EnterDropDown>k__BackingField'
    IL_002c:  ldarg.0
    IL_002d:  ldc.i4.1
    IL_002e:  stfld      bool AntdUI.Select::'<ClickSwitchDropdown>k__BackingField'
    IL_0033:  ldarg.0
    IL_0034:  ldc.i4.1
    IL_0035:  stfld      bool AntdUI.Select::'<AutoText>k__BackingField'
    IL_003a:  ldarg.0
    IL_003b:  ldc.i4.1
    IL_003c:  stfld      bool AntdUI.Select::'<WheelModifyEnabled>k__BackingField'
    IL_0041:  ldarg.0
    IL_0042:  ldc.i4.m1
    IL_0043:  stfld      int32 AntdUI.Select::selectedIndexX
    IL_0048:  ldarg.0
    IL_0049:  ldc.i4.m1
    IL_004a:  stfld      int32 AntdUI.Select::selectedIndex
    IL_004f:  ldarg.0
    IL_0050:  ldc.i4.1
    IL_0051:  stfld      bool AntdUI.Select::showicon
    IL_0056:  ldarg.0
    IL_0057:  ldc.r4     -1.
    IL_005c:  stfld      float32 AntdUI.Select::ArrowProg
    IL_0061:  ldarg.0
    IL_0062:  call       instance void AntdUI.Input::.ctor()
    IL_0067:  ret
  } // end of method Select::.ctor

  .method private hidebysig instance void 
          '<set_Expand>b__128_0'(float32 i) cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 代码大小       14 (0xe)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      float32 AntdUI.Select::ArrowProg
    IL_0007:  ldarg.0
    IL_0008:  callvirt   instance void AntdUI.Input::Invalidate()
    IL_000d:  ret
  } // end of method Select::'<set_Expand>b__128_0'

  .event AntdUI.IntEventHandler SelectedIndexChanged
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 26 53 65 6C 65 63 74 65 64 49 6E 64 65 78   // ..&SelectedIndex
                                                                                                20 E5 B1 9E E6 80 A7 E5 80 BC E6 9B B4 E6 94 B9   //  ...............
                                                                                                E6 97 B6 E5 8F 91 E7 94 9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .addon instance void AntdUI.Select::add_SelectedIndexChanged(class AntdUI.IntEventHandler)
    .removeon instance void AntdUI.Select::remove_SelectedIndexChanged(class AntdUI.IntEventHandler)
  } // end of event Select::SelectedIndexChanged
  .event AntdUI.IntXYEventHandler SelectedIndexsChanged
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 1E E5 A4 9A E5 B1 82 E6 A0 91 E7 BB 93 E6 
                                                                                                9E 84 E6 9B B4 E6 94 B9 E6 97 B6 E5 8F 91 E7 94 
                                                                                                9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [mscorlib]System.ObsoleteAttribute::.ctor(string) = ( 01 00 18 75 73 65 20 53 65 6C 65 63 74 65 64 56   // ...use SelectedV
                                                                                61 6C 75 65 43 68 61 6E 67 65 64 00 00 )          // alueChanged..
    .addon instance void AntdUI.Select::add_SelectedIndexsChanged(class AntdUI.IntXYEventHandler)
    .removeon instance void AntdUI.Select::remove_SelectedIndexsChanged(class AntdUI.IntXYEventHandler)
  } // end of event Select::SelectedIndexsChanged
  .event AntdUI.ObjectNEventHandler SelectedValueChanged
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 26 53 65 6C 65 63 74 65 64 56 61 6C 75 65   // ..&SelectedValue
                                                                                                20 E5 B1 9E E6 80 A7 E5 80 BC E6 9B B4 E6 94 B9   //  ...............
                                                                                                E6 97 B6 E5 8F 91 E7 94 9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .addon instance void AntdUI.Select::add_SelectedValueChanged(class AntdUI.ObjectNEventHandler)
    .removeon instance void AntdUI.Select::remove_SelectedValueChanged(class AntdUI.ObjectNEventHandler)
  } // end of event Select::SelectedValueChanged
  .event AntdUI.BoolEventHandler ExpandDropChanged
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 25 E4 B8 8B E6 8B 89 E5 B1 95 E5 BC 80 20   // ..%............ 
                                                                                                E5 B1 9E E6 80 A7 E5 80 BC E6 9B B4 E6 94 B9 E6 
                                                                                                97 B6 E5 8F 91 E7 94 9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .addon instance void AntdUI.Select::add_ExpandDropChanged(class AntdUI.BoolEventHandler)
    .removeon instance void AntdUI.Select::remove_ExpandDropChanged(class AntdUI.BoolEventHandler)
  } // end of event Select::ExpandDropChanged
  .event AntdUI.ObjectNEventHandler ClosedItem
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 16 E5 85 B3 E9 97 AD E6 9F 90 E9 A1 B9 20   // ............... 
                                                                                                E6 97 B6 E5 8F 91 E7 94 9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .addon instance void AntdUI.Select::add_ClosedItem(class AntdUI.ObjectNEventHandler)
    .removeon instance void AntdUI.Select::remove_ClosedItem(class AntdUI.ObjectNEventHandler)
  } // end of event Select::ClosedItem
  .event AntdUI.Select/FilterEventHandler FilterChanged
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 20 E6 8E A7 E5 88 B6 E7 AD 9B E9 80 89 20   // .. ............ 
                                                                                                54 65 78 74 E6 9B B4 E6 94 B9 E6 97 B6 E5 8F 91   // Text............
                                                                                                E7 94 9F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .addon instance void AntdUI.Select::add_FilterChanged(class AntdUI.Select/FilterEventHandler)
    .removeon instance void AntdUI.Select::remove_FilterChanged(class AntdUI.Select/FilterEventHandler)
  } // end of event Select::FilterChanged
  .property instance bool BanInput()
  {
    .get instance bool AntdUI.Select::get_BanInput()
  } // end of property Select::BanInput
  .property instance bool List()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E6 98 AF E5 90 A6 E5 88 97 E8 A1 A8 E6 
                                                                                                A0 B7 E5 BC 8F 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_List()
    .set instance void AntdUI.Select::set_List(bool)
  } // end of property Select::List
  .property instance valuetype AntdUI.TAlignFrom
          Placement()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E8 8F 9C E5 8D 95 E5 BC B9 E5 87 BA E4 
                                                                                                BD 8D E7 BD AE 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(object) = ( 01 00 55 11 41 6E 74 64 55 49 2E 54 41 6C 69 67   // ..U.AntdUI.TAlig
                                                                                                 6E 46 72 6F 6D 20 00 00 00 00 00 )                // nFrom .....
    .get instance valuetype AntdUI.TAlignFrom AntdUI.Select::get_Placement()
    .set instance void AntdUI.Select::set_Placement(valuetype AntdUI.TAlignFrom)
  } // end of property Select::Placement
  .property instance bool ListAutoWidth()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 18 E6 98 AF E5 90 A6 E5 88 97 E8 A1 A8 E8 
                                                                                                87 AA E5 8A A8 E5 AE BD E5 BA A6 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_ListAutoWidth()
    .set instance void AntdUI.Select::set_ListAutoWidth(bool)
  } // end of property Select::ListAutoWidth
  .property instance int32 MaxCount()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 18 E5 88 97 E8 A1 A8 E6 9C 80 E5 A4 9A E6 
                                                                                                98 BE E7 A4 BA E6 9D A1 E6 95 B0 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(int32) = ( 01 00 04 00 00 00 00 00 ) 
    .get instance int32 AntdUI.Select::get_MaxCount()
    .set instance void AntdUI.Select::set_MaxCount(int32)
  } // end of property Select::MaxCount
  .property instance valuetype [mscorlib]System.Nullable`1<int32>
          DropDownRadius()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 0C E4 B8 8B E6 8B 89 E5 9C 86 E8 A7 92 00 
                                                                                                00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(string) = ( 01 00 FF 00 00 ) 
    .get instance valuetype [mscorlib]System.Nullable`1<int32> AntdUI.Select::get_DropDownRadius()
    .set instance void AntdUI.Select::set_DropDownRadius(valuetype [mscorlib]System.Nullable`1<int32>)
  } // end of property Select::DropDownRadius
  .property instance bool DropDownArrow()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 18 E4 B8 8B E6 8B 89 E7 AE AD E5 A4 B4 E6 
                                                                                                98 AF E5 90 A6 E6 98 BE E7 A4 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_DropDownArrow()
    .set instance void AntdUI.Select::set_DropDownArrow(bool)
  } // end of property Select::DropDownArrow
  .property instance valuetype [System.Drawing]System.Drawing.Size
          DropDownPadding()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 0C E4 B8 8B E6 8B 89 E8 BE B9 E8 B7 9D 00 
                                                                                                00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(class [mscorlib]System.Type,
                                                                                     string) = ( 01 00 66 53 79 73 74 65 6D 2E 44 72 61 77 69 6E   // ..fSystem.Drawin
                                                                                                 67 2E 53 69 7A 65 2C 20 53 79 73 74 65 6D 2E 44   // g.Size, System.D
                                                                                                 72 61 77 69 6E 67 2C 20 56 65 72 73 69 6F 6E 3D   // rawing, Version=
                                                                                                 34 2E 30 2E 30 2E 30 2C 20 43 75 6C 74 75 72 65   // 4.0.0.0, Culture
                                                                                                 3D 6E 65 75 74 72 61 6C 2C 20 50 75 62 6C 69 63   // =neutral, Public
                                                                                                 4B 65 79 54 6F 6B 65 6E 3D 62 30 33 66 35 66 37   // KeyToken=b03f5f7
                                                                                                 66 31 31 64 35 30 61 33 61 05 31 32 2C 20 35 00   // f11d50a3a.12, 5.
                                                                                                 00 ) 
    .get instance valuetype [System.Drawing]System.Drawing.Size AntdUI.Select::get_DropDownPadding()
    .set instance void AntdUI.Select::set_DropDownPadding(valuetype [System.Drawing]System.Drawing.Size)
  } // end of property Select::DropDownPadding
  .property instance valuetype AntdUI.TAlign
          DropDownTextAlign()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E4 B8 8B E6 8B 89 E6 96 87 E6 9C AC E6 
                                                                                                96 B9 E5 90 91 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(object) = ( 01 00 55 0D 41 6E 74 64 55 49 2E 54 41 6C 69 67   // ..U.AntdUI.TAlig
                                                                                                 6E 28 00 00 00 00 00 )                            // n(.....
    .get instance valuetype AntdUI.TAlign AntdUI.Select::get_DropDownTextAlign()
    .set instance void AntdUI.Select::set_DropDownTextAlign(valuetype AntdUI.TAlign)
  } // end of property Select::DropDownTextAlign
  .property instance bool EnterDropDown()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 24 E6 98 AF E5 90 A6 E5 85 81 E8 AE B8 E5   // ..$.............
                                                                                                9B 9E E8 BD A6 E9 94 AE E8 A7 A6 E5 8F 91 E4 B8 
                                                                                                8B E6 8B 89 E6 A1 86 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
    .get instance bool AntdUI.Select::get_EnterDropDown()
    .set instance void AntdUI.Select::set_EnterDropDown(bool)
  } // end of property Select::EnterDropDown
  .property instance bool DropDownEmptyClose()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E4 B8 8B E6 8B 89 E4 B8 BA E7 A9 BA E5 
                                                                                                85 B3 E9 97 AD 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_DropDownEmptyClose()
    .set instance void AntdUI.Select::set_DropDownEmptyClose(bool)
  } // end of property Select::DropDownEmptyClose
  .property instance bool ClickEnd()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 2D E7 82 B9 E5 87 BB E5 88 B0 E6 9C 80 E9   // ..-.............
                                                                                                87 8C E5 B1 82 EF BC 88 E6 97 A0 E8 8A 82 E7 82 
                                                                                                B9 E6 89 8D E8 83 BD E7 82 B9 E5 87 BB EF BC 89 
                                                                                                00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_ClickEnd()
    .set instance void AntdUI.Select::set_ClickEnd(bool)
  } // end of property Select::ClickEnd
  .property instance bool ClickSwitchDropdown()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E7 82 B9 E5 87 BB E5 88 87 E6 8D A2 E4 
                                                                                                B8 8B E6 8B 89 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
    .get instance bool AntdUI.Select::get_ClickSwitchDropdown()
    .set instance void AntdUI.Select::set_ClickSwitchDropdown(bool)
  } // end of property Select::ClickSwitchDropdown
  .property instance bool CloseIcon()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 18 E6 98 AF E5 90 A6 E6 98 BE E7 A4 BA E5 
                                                                                                85 B3 E9 97 AD E5 9B BE E6 A0 87 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_CloseIcon()
    .set instance void AntdUI.Select::set_CloseIcon(bool)
  } // end of property Select::CloseIcon
  .property instance bool Empty()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E4 B8 BA E7 A9 BA E4 BE 9D E6 97 A7 E4 
                                                                                                B8 8B E6 8B 89 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_Empty()
    .set instance void AntdUI.Select::set_Empty(bool)
  } // end of property Select::Empty
  .property instance bool AutoPrefixSvg()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 18 E8 87 AA E5 8A A8 E8 AE BE E7 BD AE E4 
                                                                                                B8 8B E6 8B 89 E5 89 8D E7 BC 80 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_AutoPrefixSvg()
    .set instance void AntdUI.Select::set_AutoPrefixSvg(bool)
  } // end of property Select::AutoPrefixSvg
  .property instance bool AutoText()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E8 87 AA E5 8A A8 E8 AE BE E7 BD AE E6 
                                                                                                96 87 E6 9C AC 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
    .get instance bool AntdUI.Select::get_AutoText()
    .set instance void AntdUI.Select::set_AutoText(bool)
  } // end of property Select::AutoText
  .property instance bool WheelModifyEnabled()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 15 E9 BC A0 E6 A0 87 E6 BB 9A E8 BD AE E4 
                                                                                                BF AE E6 94 B9 E5 80 BC 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E4 BA A4 E4 BA 92 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
    .get instance bool AntdUI.Select::get_WheelModifyEnabled()
    .set instance void AntdUI.Select::set_WheelModifyEnabled(bool)
  } // end of property Select::WheelModifyEnabled
  .property instance class AntdUI.BaseCollection
          Items()
  {
    .custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = ( 01 00 01 00 00 ) 
    .custom instance void [System]System.ComponentModel.DesignerSerializationVisibilityAttribute::.ctor(valuetype [System]System.ComponentModel.DesignerSerializationVisibility) = ( 01 00 02 00 00 00 00 00 ) 
    .custom instance void [System]System.ComponentModel.EditorAttribute::.ctor(string,
                                                                               class [mscorlib]System.Type) = ( 01 00 3D 53 79 73 74 65 6D 2E 57 69 6E 64 6F 77   // ..=System.Window
                                                                                                                73 2E 46 6F 72 6D 73 2E 44 65 73 69 67 6E 2E 4C   // s.Forms.Design.L
                                                                                                                69 73 74 43 6F 6E 74 72 6F 6C 53 74 72 69 6E 67   // istControlString
                                                                                                                43 6F 6C 6C 65 63 74 69 6F 6E 45 64 69 74 6F 72   // CollectionEditor
                                                                                                                75 53 79 73 74 65 6D 2E 44 72 61 77 69 6E 67 2E   // uSystem.Drawing.
                                                                                                                44 65 73 69 67 6E 2E 55 49 54 79 70 65 45 64 69   // Design.UITypeEdi
                                                                                                                74 6F 72 2C 20 53 79 73 74 65 6D 2E 44 72 61 77   // tor, System.Draw
                                                                                                                69 6E 67 2C 20 56 65 72 73 69 6F 6E 3D 34 2E 30   // ing, Version=4.0
                                                                                                                2E 30 2E 30 2C 20 43 75 6C 74 75 72 65 3D 6E 65   // .0.0, Culture=ne
                                                                                                                75 74 72 61 6C 2C 20 50 75 62 6C 69 63 4B 65 79   // utral, PublicKey
                                                                                                                54 6F 6B 65 6E 3D 62 30 33 66 35 66 37 66 31 31   // Token=b03f5f7f11
                                                                                                                64 35 30 61 33 61 00 00 )                         // d50a3a..
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 06 E9 9B 86 E5 90 88 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E6 95 B0 E6 8D AE 00 00 ) 
    .get instance class AntdUI.BaseCollection AntdUI.Select::get_Items()
    .set instance void AntdUI.Select::set_Items(class AntdUI.BaseCollection)
  } // end of property Select::Items
  .property instance int32 SelectedIndex()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 0C E9 80 89 E4 B8 AD E5 BA 8F E5 8F B7 00 
                                                                                                00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E6 95 B0 E6 8D AE 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(int32) = ( 01 00 FF FF FF FF 00 00 ) 
    .get instance int32 AntdUI.Select::get_SelectedIndex()
    .set instance void AntdUI.Select::set_SelectedIndex(int32)
  } // end of property Select::SelectedIndex
  .property instance object SelectedValue()
  {
    .custom instance void [System]System.ComponentModel.BrowsableAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 09 E9 80 89 E4 B8 AD E5 80 BC 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E6 95 B0 E6 8D AE 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(string) = ( 01 00 FF 00 00 ) 
    .get instance object AntdUI.Select::get_SelectedValue()
    .set instance void AntdUI.Select::set_SelectedValue(object)
  } // end of property Select::SelectedValue
  .property instance bool ShowIcon()
  {
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E6 98 AF E5 90 A6 E6 98 BE E7 A4 BA E5 
                                                                                                9B BE E6 A0 87 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 01 00 00 ) 
    .get instance bool AntdUI.Select::get_ShowIcon()
    .set instance void AntdUI.Select::set_ShowIcon(bool)
  } // end of property Select::ShowIcon
  .property instance bool HasSuffix()
  {
    .get instance bool AntdUI.Select::get_HasSuffix()
  } // end of property Select::HasSuffix
  .property instance bool Expand()
  {
    .custom instance void [System]System.ComponentModel.BrowsableAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 16 E7 AE AD E5 A4 B4 E6 98 AF E5 90 A6 E5 
                                                                                                B1 95 E5 BC 80 28 55 49 29 00 00 )                // .....(UI)..
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E5 A4 96 E8 A7 82 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_Expand()
    .set instance void AntdUI.Select::set_Expand(bool)
  } // end of property Select::Expand
  .property instance bool ExpandDrop()
  {
    .custom instance void [System]System.ComponentModel.BrowsableAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .custom instance void [System]System.ComponentModel.DescriptionAttribute::.ctor(string) = ( 01 00 12 E5 B1 95 E5 BC 80 E4 B8 8B E6 8B 89 E8 
                                                                                                8F 9C E5 8D 95 00 00 ) 
    .custom instance void [System]System.ComponentModel.CategoryAttribute::.ctor(string) = ( 01 00 06 E8 A1 8C E4 B8 BA 00 00 ) 
    .custom instance void [System]System.ComponentModel.DefaultValueAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
    .get instance bool AntdUI.Select::get_ExpandDrop()
    .set instance void AntdUI.Select::set_ExpandDrop(bool)
  } // end of property Select::ExpandDrop
} // end of class AntdUI.Select

// *********** 反汇编完成 ***********************
