using System.Windows.Input;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Models;
using InspectionNet.Core.Views;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface ICameraViewModel : IBaseViewModel
    {
        /// <summary>
        /// 사용 가능한 Grabber Maker 목록입니다.
        /// </summary>
        IList<GrabberMaker> GrabberMakers { get; }

        /// <summary>
        /// 현재 선택된 Grabber Maker입니다.
        /// </summary>
        GrabberMaker SelectedGrabberMaker { get; set; }

        /// <summary>
        /// 사용 가능한 카메라 목록입니다.
        /// </summary>
        IEnumerable<string> CameraIdList { get; set; }

        /// <summary>
        /// 현재 선택된 카메라입니다.
        /// </summary>
        string SelectedCameraId { get; set; }

        /// <summary>
        /// Laon Display 객체입니다.
        /// </summary>
        ILpDisplay LaonDisplay { get; }

        /// <summary>
        /// 카메라 검색 명령입니다.
        /// </summary>
        ICommand DiscoverCommand { get; }

        /// <summary>
        /// 카메라 열기 명령입니다.
        /// </summary>
        ICommand OpenCommand { get; }

        /// <summary>
        /// 카메라 닫기 명령입니다.
        /// </summary>
        ICommand CloseCommand { get; }

        /// <summary>
        /// 이미지 캡처 시작 명령입니다.
        /// </summary>
        ICommand GrabStartCommand { get; }

        /// <summary>
        /// 이미지 캡처 중지 명령입니다.
        /// </summary>
        ICommand GrabStopCommand { get; }

        /// <summary>
        /// 저장 디렉토리 선택 명령입니다.
        /// </summary>
        ICommand SelectDirectoryCommand { get; }

        /// <summary>
        /// 이미지 저장 명령입니다.
        /// </summary>
        ICommand SaveImageCommand { get; }
        IList<IGenApiParameter> SelectedCameraParameters { get; set; }
    }
}
