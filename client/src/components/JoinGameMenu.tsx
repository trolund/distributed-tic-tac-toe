import { useForm, SubmitHandler } from "react-hook-form";

export type JoinFromData = {
  playerName: string;
  gameId: string;
};

type JoinGameMenuProps = {
  onSubmit: (data: JoinFromData) => void;
};

export default function JoinGameMenu({
  onSubmit: SubmitHandler,
}: JoinGameMenuProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<JoinFromData>();
  const onSubmit: SubmitHandler<JoinFromData> = (data) => SubmitHandler(data);
  return (
    <form
      className="ml-auto mr-auto flex max-w-96 flex-col justify-center gap-4"
      onSubmit={handleSubmit(onSubmit)}
    >
      <label htmlFor="gameId">Game join code</label>
      <input
        className="rounded-lg bg-slate-200 p-2 text-black"
        {...register("gameId", { required: true })}
      />
      <label htmlFor="playerName">Player name</label>
      {errors.gameId && (
        <span className="text-red-900">
          A game ID is required to join a game
        </span>
      )}
      <input
        className="rounded-lg bg-slate-200 p-2 text-black"
        {...register("playerName", { required: true })}
      />
      {errors.playerName && (
        <span className="text-red-900">A player nickname is required</span>
      )}
      <button type="submit">Join game</button>
    </form>
  );
}
