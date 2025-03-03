import { ThemedView } from '@/components/ui/ThemedView';
import UserProfileHeader from '@/components/modules/user/UserProfileHeader';
import { useUserAuth } from '@/contexts/UserAuthenticationContext';

const UserScreen = () => {
	const { userData } = useUserAuth();

	// eslint-disable-next-line no-console
	console.log('userData', userData);

	return (
		<ThemedView className="flex-1 w-full p-5">
			<ThemedView className="flex items-center justify-center w-full gap-4">
				<UserProfileHeader username={userData?.name} email={userData?.email} />
			</ThemedView>
		</ThemedView>
	);
};

export default UserScreen;