# AimingLib �ȉ��̃R�[�h�� Assets/Aiming �ɃR�s�[
# Test ����n�܂�t�H���_�[�ƁASample ����n�܂�t�H���_�[�͏��O
# .cs �t�@�C���� .meta �t�@�C���ȊO�͍폜

$testProject = 'Test*'
$sampleProject = 'Sample*'
$excludeFolders = 'Properties', 'bin', 'obj'

$from = '.'
$to = '..\Assets\Aiming'

$projects = ls |
    ?{ -not ($_.Name -like $testProject) } |
    ?{ -not ($_.Name -like $sampleProject) } |
    ?{ $_ -is [IO.DirectoryInfo] }

# ��������ۂ��ƃR�s�[
foreach($p in $projects)
{
    copy $p.FullName $to -Force -Recurse
}

# bin �Ƃ��̃t�H���_�[�͂���
foreach($ex in $excludeFolders)
{
    ls $to -Recurse | ?{ $_.Name -eq $ex } | ?{ $_ -is [IO.DirectoryInfo] } | rm -Recurse
}

# .cs �ȊO�s�v�Ȃ̂ō폜
ls $to -Recurse -Force | ?{ ($_.Extension -ne '.cs') -and ($_.Extension -ne '.meta') } | ?{ $_ -is [IO.FileInfo] } | rm -Force
