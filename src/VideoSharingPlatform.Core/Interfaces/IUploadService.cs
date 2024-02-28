namespace VideoSharingPlatform.Core.Interfaces;

public interface IUploadService<TFile> {
    Task UploadVideoAsync(TFile file, string path, string name);
    Task UploadImageAsync(TFile file, string path, string name);
}