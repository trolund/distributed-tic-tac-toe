type ConnectionBarProps = {
  isconnected: boolean;
};

export default function ConnectionBar({ isconnected }: ConnectionBarProps) {
  return (
    <>
      {isconnected ? (
        <p className="text-green-500">Connected</p>
      ) : (
        <p className="text-red-500">Not connected</p>
      )}
    </>
  );
}
