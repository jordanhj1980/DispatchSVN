   M S B A M L   `   `   `  ���� F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:DispatchApp.ConvertersDispatchApp.Converters  )clr-namespace:DispatchAppDispatchApp  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0893    -DispatchApp.Converters.LevelToIndentConverter   LevelIndentConverter  ExpandCollapseToggleStyle  �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius J� SnapsToDevicePixels �� 
StartPoint �� EndPoint � Color	 � Offset
 F�Name �� Triggers P� Property P� Value �� 
TargetName  CellTemplate_Name d�Name �� 	ClickMode �� Path �� 	Converter �� RelativeSource �� AncestorType"   DispatchApp.TreeListViewItem d� 	IsChecked �� Triggers �� Value �� Binding  gvHeaderBackground �� ImageSource  gvColumnHeaderStyle6  0System.Windows.Controls.GridViewColumnCollection  	gvColumns �� CellTemplate �� Width �� DisplayMemberBinding ��Name �� Padding ��Name  �� Content! �� Columns" ��Name# P� 
SourceName$ ]� 
Conditions% �� Property& �� Value   DispatchApp.TreeListView!' �� ColumnHeaderContainerStyle�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     0convert$clr-namespace:DispatchApp.Converters  #localclr-namespace:DispatchApp  %L
  &	        &	      &	 E    7  7   &	 1    &	 p    7  &	 �    7  '  ^    7  '  '	    7  7     �� "  d�.���  � ��$ 19����  � ��$ 19����  � |� .  ��  � �� �� " d�.R��� $��19��$��19��
������ ._��� $��19��$��19��$��1q����	#FF7898B5$ 1�� . ���� $	 0,0&�$	 1,1&���� $ White��$	 .2Q�� $ 	#FFC0B7A6��$	 1Q�._�F� $
 
ExpandPath��$��1,1,1,1q�
���   �s����    �� �    ��- р� ��- р� @KL ���- @KL ���- ��- �@KL ��- �@KL �� ���- �� ���-     р�     р� �� 	 P�  �@d�	IsChecked$
 TrueY�. ��� $ 
ExpandPath�� � ��+ ��    �� �    ��- �@KL ��- �@KL �� 	�� .R�]� .+�d� $ Expander��8��  $ Press=����� $ Level�8   �� "   �� $ 
IsExpanded� �� "   �� $ False�� �� $ HasItems� �� "  .a��� $ Expander�� � y�$ Hidden=��� $9 3pack://application:,,,/Resources/list_header_bk.png���� "  �����  � ��$ 34����  � ��8     �� $�� ��8   $ 20���� $��组名��$	 100�� �� *
department+�� $��电话号码��$	 100�� �� *callno+�� $��标识��$	 100�� �� *name+�� "   .���  � ��$ 18(���  � ��
 ���� ��  �@J�SnapsToDevicePixels . ��  � ��$ Stretch=���  � ��$ Center=���  �@Q�
IsSelected �� *
IsSelected+��  �@Q�
IsExpanded �� *
IsExpanded+��  � �� �� "  .R��� .+��� $ outerBorder��$��0,0,0,1q����	#FFFFFFFD$��0q�$ 0q�$��30��#��z�� . ._��� $ innerBorder��$��0,0,0,1q����	#FFC6C6C6#��z��# z��#��z�� . ._��� $ PART_Header��#  z��8!   #��z��#��z���� $" 	ItemsHost�� P� $# innerBorder�� �@J�IsMouseOver . . ���  � ��
 ������� $ innerBorder�� � �� �	#FFC6C6C6P�  �@Q�
IsSelected . . ���  � ��
 ������� $ innerBorder�� � �� �	#FFC6C6C6P�  �@Q�
IsExpanded .  . ��� $ 	ItemsHost�� � y�$ 	Collapsed=�P�  � {� .  . ��� # � �� � ��]� $ �� % �@Q�
IsSelected& . �� % �@Q�IsSelectionActive& .  ./��� $ innerBorder��# � �� � ���� # � �� � ��]� $ �� % � ��& .  �� % � ��$
& Auto��./��� $ PART_Header�� � ��$ 75��]� $ �� % � ��& .  �� % � ��$
& Auto��./��� $ PART_Header�� � ��$ 19���� "   .���  � �� �� "  .R��� # z��#��z��#��z��#��z��._��� $
��Auto=�$��Disabled=�.��]� .+��� $	��Top=�8'   8!  �� $��Disabled=�$
��Auto=�.���� 