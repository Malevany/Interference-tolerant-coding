   M S B A M L   `   `   `  ����_  [Курсовая работа ТиК, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ГрафичВып1ГрафичВып1  5   	   $    ГрафичВып1.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:ГрафичВып1     5   	   $	��429��5        � Title$  ShannonCodeWindow��5   	   $	��740��6#   .��5	      � 
������� � ColumnDefinitions 5
   
   �� 5      $
��323* �6   �� 5      $
��417* �6   �� 5      $��0* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ AlphabetDataGrid��5         AutoGenerateColumns$
 True��6-   $��1��6H   �� 5   
   $
��Left=�6   $��Center=�60    �� Orientation$ Vertical=�6K   .+�5      �� -   5         ��Name$ InputMessageAndButtons��5      $ Vertical=�69   .+�5      �� -   5         ��Name$ InputMessage_TextBox��5       �� TextWrapping$
 Wrap=�68   $	��232��6L   $��20��6X   �� 5      -   5        $ EncryptClearButtons��5      $ 
Horizontal=�6:   $	��232��6S   .+�5      �� -   5         ��Name$ EncryptButton��5      $��Рассчитать��64   $��96��6I   $��20��6T   $��10q�6`   6l   �� 5      -   5        $ ClearButton��5      $��Очистить��64   $��96��6I   $��20��6T   $��10q�6`   6l   5      5      �� 5      -   5        $, &StatisticalCodeCharacteristics_Content��5      $ 
Horizontal=�6I   .+�5      �� -   5        $+ %StatisticalCodeCharacteristics_Labels��5      $ Vertical=�6L   .+�5      �� -	   5        	 ��Name$	 ShannonEntropy_Label��5      $+��%Энтропия Шеннона H(x):��6:   $��30��6i   $	��190��6u   $��14(�6�   �� 5      -
   5        $	 CodeRedundancy_Label��5      $*��$Избыточность кода R:��6:   $��30��6i   $	��190��6u   $��14(�6�   �� 5      -   5        $%	 AverageWeightedCodeLength_Label��5      $/��)Срвзвыш.длина кода Lср:��6E   $��30��6i   $	��190��6u   $��14(�6�   �� 5      -   5        $"	 RelativeCodeRedundancy_Label��5      $2��,Отн.избыточность кода pk:��6B   $��30��6i   $	��190��6u   $��14(�6�   �� 5      -   5        $	 CodeOptimality_Label��5      $!��Оптимальность:��6:   $��30��6i   $	��190��6u   $��14(�6�   5      �� 5       -   5        $, &StatisticalCodeCharacteristics_TextBox��5       $ Vertical=�6M   .+�5!      �� -   5        $ ShannonEntropy_TextBox��5!      $
 Wrap=�6I   
 �� TextAlignment$
 Center=�6]   $��20��6t   $	��125��6�   $��0,5,0,5q�6�   �� 5"      -   5        $ CodeRedundancy_TextBox��5"      $
 Wrap=�6I   $
 Center=�6]   $��20��6t   $	��125��6�   $��0,5,0,5q�6�   �� 5#      -   5        $' !AverageWeightedCodeLength_TextBox��5#      $
 Wrap=�6I   $
 Center=�6]   $��20��6t   $	��125��6�   $��0,5,0,5q�6�   �� 5$      -   5        $$ RelativeCodeRedundancy_TextBox��5$      $
 Wrap=�6I   $
 Center=�6]   $��20��6t   $	��125��6�   $��0,5,0,5q�6�   �� 5%      -   5        $ CodeOptimality_TextBox��5%      $
 Wrap=�6I   $
 Center=�6]   $��20��6t   $	��125��6�   $��0,5,0,5q�6�   5&      5'      �� 5(      -   5        $ EncryptMessage_Content��5(      $ Vertical=�69   .+�5)      �� -   5        $	 EncryptMessage_Label��5)      $6��0Закодированное сообщение:��66   $��30��6i   $	��200��6u   $��14(�6�   �� 5*      -   5        $ EncryptMessage_TextBox��5*      $
 Wrap=�6:   $	��200��6N   $��75��6Z   5+      �� 5,      -   5        $ HammingCodeButton��5,      $��Код Хэмминга��60   $	��150��6G   $��30��6S   $��10q�6_   $��20(�6�   5-      5/      50      