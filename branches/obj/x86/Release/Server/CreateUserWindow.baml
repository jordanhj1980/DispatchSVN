   M S B A M L   `   `   `  ����   DispatchApp"    DispatchApp.CreateUserWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  添加用户��$	��484��$	��366��.��� $	��448��$��Center=�$	��Top=�$	��344��.+��� -   $��tabControl1��$	��400��$��5,5,0,0q�$	��Top=�.@��� -   $��tabItem1��$��常规��.���� $	��369��.+�� $	��369�� � ColumnDefinitions �� $	��62* ��� $
��200* � � RowDefinitions �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��70* �.+��� -   $��label1��$��	标识：��$��Right=�$��Center=��� -   $��label2��$��	姓名：��$��Right=�$��Center=�$��1���� -   $��label3��$��	密码：��$��Right=�$��Center=�$��2���� -   $��label4��$��确认密码：��$��Right=�$��Center=�$��3���� -   $��label5��$��	状态：��$��Right=�$��Center=�$��4���� -   $��label7��$��	权限：��$��Right=�$��Center=�$��5���� -	   $��label8��$��调度台：��$��Right=�$��Center=�$��6���� -
   $��label9��$��	描述：��$��Right=�$��Center=�$��7���� -   $��tb_id��$��1��$
��Left=�$��Center=�$	��217��$��0,1,0,3q��� -   $��tb_name��$��1��$��1��$
��Left=�$��Center=�$	��218���� -   $��tb_pass��$��1��$��2��$
��Left=�$��Center=�$	��217���� -   $��
tb_passure��$��1��$��3��$
��Left=�$��Center=�$	��217���� -   $��comBox_status��$��1��$��4��$
��Left=�$��Center=�$	��215�� � DataContext �� �� -   $��comBox_privilege��$��1��$��5��$
��Left=�$��Center=�$	��215���� -   $��comBox_desk��$��1��$��6��$
��Left=�$��Center=�$	��215���� -   $��tb_description��$
��Left=�$��Center=�$	��218��$��1��$��7��$��88��$��0,0,0,1q� �� TextWrapping$
 Wrap=���  �� Orientation$ 
Horizontal=�$��81,411,0,-1q�.+��� -   $��btn_ok��$��确定��$��23��$��75��$��10,0,0,0q��� -   $��
btn_cancel��$��取消��$��23��$��75��$��10,0,0,0q��� -   $��	btn_apply��$��应用��$��23��$��75��$��10,0,0,0q�