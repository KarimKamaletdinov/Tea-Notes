namespace Tea_Notes
{
    interface IFolderRepository
    {
        void Save(FolderDTO dto);

        FolderDTO[] GetAll();

        void Delete(FolderDTO dto);
    }
}
