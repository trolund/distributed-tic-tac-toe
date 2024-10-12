import { useForm, SubmitHandler } from "react-hook-form";

export type CreateFromData = {
  playerName: string;
};

type CreateGameMenuProps = {
  onSubmit: (data: CreateFromData) => void;
};

export default function CreateGameMenu({
  onSubmit: SubmitHandler,
}: CreateGameMenuProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateFromData>();
  const onSubmit: SubmitHandler<CreateFromData> = (data) => SubmitHandler(data);
  return (
    <form
      className="ml-auto mr-auto flex max-w-96 flex-col justify-center gap-4"
      onSubmit={handleSubmit(onSubmit)}
    >
      <label htmlFor="playerName">Player name</label>
      <input
        className="rounded-lg bg-slate-200 p-2 text-black"
        {...register("playerName", { required: true })}
      />
      {errors.playerName && (
        <span className="text-red-900">A player name is required</span>
      )}
      <button type="submit">Create new game</button>
    </form>
  );
}
