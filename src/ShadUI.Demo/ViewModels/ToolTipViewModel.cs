using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public sealed partial class ToolTipViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _usageCode ="""
                               <StackPanel VerticalAlignment="Center" HorizontalAlignment="Center">
                                   <Button ToolTip.Tip="Add to library"
                                           ToolTip.Placement="Top"
                                           ToolTip.VerticalOffset="-5"
                                           Classes="Outline">
                                       Hover
                                   </Button>
                               </StackPanel>
                               """;
}